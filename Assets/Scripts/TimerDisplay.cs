using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerDisplay : MonoBehaviour
{

    private int timer = 0;
    public TMP_Text timerText;
    private bool continueTimer = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timerText.text = "TIME: " + timer; 
    }

    // increase the timer based on a trigger event
    public void TimerIncrease()
    {
        if(continueTimer)
        {
            timer++;
        }
        
    }

    // pause the timer (triggered by reaching the end)
    public void setTimerFalse ()
    {
        continueTimer = false;
    }
}

