using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using UnityEngine.Events;
using TMPro;
public class Movement : MonoBehaviour
{

    [SerializeField] private float speed = 30f;
    [SerializeField] private Animator animator;
    [SerializeField] private LayerMask ground;
    [SerializeField] private LayerMask obstacles;
    [SerializeField] private float jumpForce = 10.0f;
    [SerializeField] private float doubleJumpForce = 2.0f;
    [SerializeField] private float maxSpeed = 100.0f;
    [SerializeField] private float acceleration = 10.0f;
    [SerializeField] private float deceleration = 5.0f;
    [SerializeField] private float groundFriction = 0.3f;
    private float xMovement;
    private float _currentVelocity;
    private float _currentVelocityY;
    private Rigidbody2D _rigidbody;
    private Collider2D _collider;
    static readonly int IsRunning = Animator.StringToHash("IsRunning");
    static readonly int IsJumping = Animator.StringToHash("IsJumping");
    static readonly int IsFalling = Animator.StringToHash("IsFalling");
    static readonly int IsRising = Animator.StringToHash("IsRising");
    static readonly int DownHeld = Animator.StringToHash("DownHeld");
    static readonly int TouchingGround = Animator.StringToHash("IsGrounded");
    private bool hasDoubleJump = true;
    private float airFriction = 1.99f;
    private float airModifier = 0.1f;
    private bool hasMainJump = true;
    private bool downHeld = false;
    private bool downCharged = true;
    [SerializeField] private float downOffset = 1.0f;
    public AudioClip jumpSound;
    public AudioClip interruptSound;
    public AudioClip finishSound;
    [SerializeField] private GameObject _sprites;
    [SerializeField] private float downwardForce = 2.0f;
    float startX = 0;
    float startY = 0;
    private bool checkpointFlag = true;
    private bool notCompleted = true;
    [SerializeField] private UnityEvent _trigger;
    [SerializeField] private UnityEvent _completionTrigger;

    public TMP_Text finalText;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        startX = _rigidbody.position.x;
        startY = _rigidbody.position.y;
    }
    private void Update()
    {

        // update velocity
        _rigidbody.velocity = new Vector2(_currentVelocity, _rigidbody.velocity.y);

        // death check
        deathCollisionCheck();
        
        // velocity tracking
        TrackVelocity();
        //Debug.Log(_currentVelocity);


    }

    // moving left or right
    public void OnMove(InputAction.CallbackContext ctx)
    {


        float modifier = 1;
        xMovement = ctx.ReadValue<float>();

        // calculate moving direction
        if (this.IsGrounded())
        {
            float speedToAdd = xMovement * speed * modifier;

            if (ctx.ReadValue<float>() > 0.5)
            {
                _currentVelocity = speedToAdd;
            }
            else if (ctx.ReadValue<float>() < -0.5)
            {
                _currentVelocity = speedToAdd;
            }
        }
        else
        {
            float speedToAdd = xMovement * speed * modifier;

            if (ctx.ReadValue<float>() > 0.5)
            {
                _currentVelocity = speedToAdd;
            }
            else if (ctx.ReadValue<float>() < -0.5)
            {
                _currentVelocity = speedToAdd;
            }
        }
    
        //Debug.Log(ctx.ReadValue<float>());
        
        
        
        // change state to running
        if (Mathf.Abs(_currentVelocity) > 0.01f)
        {
            animator.SetBool(IsRunning, true);
        }
        else
        {
            animator.SetBool(IsRunning, false);
        }

        
    }

    // jumping
    public void OnJump(InputAction.CallbackContext ctx)
    {
        
        // check if player is grounded
        if (_collider.IsTouchingLayers(ground) && ctx.performed)
        {

            SoundFXManager.Instance.PlayClip(jumpSound, transform, 0.1f);
            Vector3 jump = new Vector3(0, 1, 0);
            
            _rigidbody.AddForce(jump * jumpForce, ForceMode2D.Impulse);
            
        }

        // check if player can double jump
        else if(!(_collider.IsTouchingLayers(ground)) && hasDoubleJump && ctx.performed)
        {
            SoundFXManager.Instance.PlayClip(jumpSound, transform, 0.1f);
            // set velocity to 0 before landing
            //Debug.Log("Can Double jump");
            Vector3 vel = _rigidbody.velocity;
            vel.y = 0;
            _rigidbody.velocity = vel;
            Vector3 jump = new Vector3(0, 1, 0);
            _rigidbody.AddForce(jump * doubleJumpForce, ForceMode2D.Impulse);
            hasDoubleJump = false;

            DirectionCheck(false);

        }



    }

    // fast land mechanics
    public void OnDown(InputAction.CallbackContext ctx)
    {   
        // if down is pressed, check for down held
        if (ctx.performed)
        {
            downHeld = true;
            animator.SetBool(DownHeld, true);
            
            _currentVelocity = 0;

            // if player is in the air + can hold down, perform a fast land
            if (downCharged && !(this.IsGrounded()))
            {
                SoundFXManager.Instance.PlayClip(interruptSound, transform, 0.3f);
                //Debug.Log("Down charge lost");
                downCharged = false;
                Vector3 vel = _rigidbody.velocity;
                vel.y = 0;
                _rigidbody.velocity = vel;
                Vector3 jump = new Vector3(0, -1, 0);
                _rigidbody.AddForce(jump * downwardForce, ForceMode2D.Impulse);
                Vector3 position = _sprites.transform.position;
                Vector3 offsetVector = new Vector3(0, -1 * downOffset, 0);
                _sprites.transform.position = position + offsetVector;
            }
            

        }
        else if (ctx.canceled)
        {
            downHeld = false;                                                                   
            animator.SetBool(DownHeld, false);
            
        }
    }
    private void TrackVelocity()
    {


        // face left if moving left, face right if moving right
        DirectionCheck(true);

        

        // track if rising
        if (_rigidbody.velocity.y > 0.01)
        {
            animator.SetBool(IsRising, true);
        }

        // track if falling
        float fallVelocity = _rigidbody.velocity.y;
        if (fallVelocity < -0.01)
        {
            animator.SetBool(IsRising, false);
            animator.SetBool(IsFalling, true);
        }
        else
        {
            //hasDoubleJump = true;
            animator.SetBool(IsFalling, false);
        }

        // use for animations
        bool isRising = animator.GetBool(IsRising);
        bool isFalling = animator.GetBool(IsFalling);

        // refresh the players double jump
        if (_collider.IsTouchingLayers(ground))
        {
            hasDoubleJump = true;
        }

        // reset the down state of the player
        if (IsGrounded() && !(downCharged))
        {
            downCharged = true;
            Vector3 position = _sprites.transform.position;
            Vector3 offsetVector = new Vector3(0, downOffset, 0);
            _sprites.transform.position = position + offsetVector;
            //Debug.Log("Down charge lost");

        }

        // apply ground friction
        if (_collider.IsTouchingLayers(ground) && Mathf.Abs(xMovement) < 0.1f)
        {
            //Debug.Log(xMovement);
            _currentVelocity *= groundFriction;
            if (Mathf.Abs(_currentVelocity) < 3f)
            {
                animator.SetBool(IsRunning, false);
                _currentVelocity = 0;
            }
        }

        // set grounded bool
        animator.SetBool(TouchingGround, IsGrounded() );


    }

    // booleans for different states
    bool IsGrounded ()
    {
        return _collider.IsTouchingLayers(ground);
    }

    bool IsTouchingObstacles()
    {
        return _collider.IsTouchingLayers(obstacles);
    }

    bool IsBelowDeathBarrier()
    {
        return _rigidbody.position.y < -20.0;
    }

    bool IsPastCheckpoint()
    {
        return _rigidbody.position.x > 130;
    }

    bool IsCompleted()
    {
        return _rigidbody.position.x > 225;
    }

    // check the direction of the player
    void DirectionCheck (bool checkGrounded)
    {
        float directionVelocity = _rigidbody.velocity.x;
        Vector3 localScale = transform.localScale;

        bool changeDirectionCheck = true;
        if (checkGrounded)
        {
            changeDirectionCheck = this.IsGrounded();
        }
        if (directionVelocity > 0 && changeDirectionCheck)
        {
            localScale = new Vector3(-1, localScale.y, localScale.z);
        }
        else if (directionVelocity < 0 && changeDirectionCheck)
        {
            localScale = new Vector3(1, localScale.y, localScale.z);
        }

        transform.localScale = localScale;
    }

    // check for death states + completion states
    void deathCollisionCheck()
    {   
        // if touches an obstacle
        if (IsTouchingObstacles())
        {
            Debug.Log("Touching obstacles");
            Vector2 position = new Vector2(startX, startY);
            _rigidbody.position = position;
        }
        // if is below stage
        if (IsBelowDeathBarrier())
        {
            Debug.Log("Touching obstacles");
            Vector2 position = new Vector2(startX, startY);
            _rigidbody.position = position;
        }
        // if reaches checkpoint
        if (checkpointFlag && IsPastCheckpoint())
        {
            checkpointFlag = false;
            Debug.Log("Touching obstacles");
            startX = 130;
            startY = 4;
            _currentVelocity = 0;
            _trigger.Invoke();
        }
        // if stage is completed
        if (IsCompleted() && notCompleted)
        {
            finalText.text = "Level completed!";
            SoundFXManager.Instance.PlayClip(finishSound, transform, 0.3f);
            notCompleted = false;
            Debug.Log("completed");
            _completionTrigger.Invoke();
        }
    }
}
