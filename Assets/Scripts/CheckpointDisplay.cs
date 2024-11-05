using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class CheckpointDisplay : MonoBehaviour
{

    private int timer = 0;
    public TMP_Text checkpointText;
    public string currentCheckpoint = "test";
    [SerializeField] AudioClip checkpointSound;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        checkpointText.text = currentCheckpoint;
    }

    public void checkpointReached()
    {
        currentCheckpoint = "CHECKPOINT REACHED!";
        float duration = 2f;
        StartCoroutine(TestRoutine(duration));
        SoundFXManager.Instance.PlayClip(checkpointSound, transform, 0.2f);

    }

    public void endingReached()
    {
        currentCheckpoint = "LEVEL COMPLETED!";
        //float duration = 3f;
        //StartCoroutine(TestRoutine(duration));

    }

    //used to wait
    IEnumerator TestRoutine(float duration)
    {
        Debug.Log($"Started at {Time.time}, waiting for {duration} seconds");
        yield return new WaitForSeconds(duration);
        Debug.Log($"Ended at {Time.time}");
        currentCheckpoint = "";
    }
}