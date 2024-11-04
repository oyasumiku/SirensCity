using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScuffedBeats : MonoBehaviour
{
    [SerializeField] private int numBeat = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void beatIncrement ()
    {
        numBeat++;
        if (numBeat > 4)
        {
            numBeat = 1;
        }
    }
}
