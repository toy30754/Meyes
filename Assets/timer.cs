using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class timer : MonoBehaviour {

    private string timerText;
    private float temp;
    public Text timer_text;
    public bool timerStr = false;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (timerStr)
        {
            temp += Time.deltaTime;
            TimeSpan timeSpan = TimeSpan.FromSeconds(temp);
            timerText = string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
            timer_text.text = timerText;
        }        
    }
}
