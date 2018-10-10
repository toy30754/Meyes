using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class time : MonoBehaviour {
    public Image morning;
    public Image afternoom;
    public Image night;

    DateTime Now = DateTime.Now;
    // Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
        change();

    }
    public int now_day()
    {
        return Now.Date.Day;
    }
    public int now_month()
    {
        return Now.Date.Month;
    }
    public int now_hour()
    {
        return Now.Hour;
    }
    public int now_year()
    {
        return Now.Year;
    }
    public void change()
    {
       
        
        //string year = Now.Year.ToString();
        //string week = Now.Date.Month.ToString();
        //string day = Now.Date.Day.ToString();
        int hour = Now.Hour;
        if (hour >= 6 && hour < 15)
        {
            morning.gameObject.SetActive(true);
            afternoom.gameObject.SetActive(false);
            night.gameObject.SetActive(false);
        }
        else if (hour>=15&&hour<19)
        {
            morning.gameObject.SetActive(false);
            afternoom.gameObject.SetActive(true);
            night.gameObject.SetActive(false);
        }
        else
        {
            morning.gameObject.SetActive(false);
            afternoom.gameObject.SetActive(false);
            night.gameObject.SetActive(true);
        }
       
    }
}
