using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerDisplay : MonoBehaviour
{

    private int timer = 0;
    public TMP_Text timerText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timerText.text = "TIME: " + timer; 
    }

    public void TimerIncrease()
    {
        timer++;
    }
}

