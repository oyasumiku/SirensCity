using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public void OnMoveInput(InputAction.CallbackContext context)
    {
        Debug.Log("OnMoveInput");
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        Debug.Log("OnJumpInput");
    }
}