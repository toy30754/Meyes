using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Old_Dropdown_month : MonoBehaviour {

    List<string> month = new List<string>() {"January","February","March","April","May","June","July","August","September","October","November","December" };
   
    public Dropdown dropdown_mpnth;
    public Text power_text;
    public Text today,today1,today2,today_1,today_2;
    public Button upper, lower;
    public int mm, dd;
    DateTime Nows = DateTime.Now;
   
    time timer;
    // Use this for initialization
    void Start () {
        PopuloateList();
        mm = Nows.Date.Month;
        dd = Nows.Date.Day;
        day_lock(dd,mm);
    }
	
	// Update is called once per frame
	void Update () {

    }
    public void text_out(string tt)
    {
        power_text.text = tt;
    }

   
    public void Dropdown_change(int index)
    {
        index = index + 1;
        mm = index;
        if ((index == 4 || index == 6 || index == 9 || index == 11)&&dd==31)
        {
            dd = 30;
        }
        else if (index == 2&&(dd==30||dd==31))
        {
            dd = 28;
        }
        day_lock(dd, mm);

    }

    void PopuloateList()
    {
        int now_month = Nows.Date.Month;
        dropdown_mpnth.AddOptions(month);
        dropdown_mpnth.value= now_month-1;
    }

    public void add()
    {
        if (mm == 1 || mm == 3 || mm == 5 || mm == 7 || mm == 8 || mm == 10 || mm == 12)
        {
            if (dd < 31)
            {
                dd++;
                day_lock(dd, mm);
            }

        }
        else if(mm == 4 || mm == 6 || mm == 9 || mm == 11 )
        {
            if (dd < 30)
            {
                dd++;
                day_lock(dd, mm);
            }
        }else if (mm==2)
        {
            if (dd < 28)
            {
                dd++;
                day_lock(dd, mm);
            }
        }
    }
    public void less()
    {
            if (dd > 1)
            {
                dd--;
                day_lock(dd,mm);
            }
        
        
    }
 

    void day_lock(int dd,int mm)
    {

        if (mm == 1 || mm == 3 || mm == 5 || mm == 7 || mm == 8 || mm == 10 || mm == 12)
        {
            if (dd == 30)
            {
                today.text = dd.ToString();
                today1.text = (dd + 1).ToString();
                today2.text = "";
                today_1.text = (dd - 1).ToString();
                today_2.text = (dd - 2).ToString();
            } else if (dd == 2)
            {
                today.text = dd.ToString();
                today1.text = (dd + 1).ToString();
                today2.text = (dd + 2).ToString();
                today_1.text = (dd - 1).ToString();
                today_2.text = "";
            } else if (dd == 1)
            {
                today.text = dd.ToString();
                today1.text = (dd + 1).ToString();
                today2.text = (dd + 2).ToString();
                today_1.text = "";
                today_2.text = "";
            }
            else if (dd == 31)
            {
                today.text = dd.ToString();
                today1.text = "";
                today2.text = "";
                today_1.text = (dd - 1).ToString();
                today_2.text = (dd - 2).ToString();
            } else if (dd < 30)
            {
                today.text = dd.ToString();
                today1.text = (dd + 1).ToString();
                today2.text = (dd + 2).ToString();
                today_1.text = (dd - 1).ToString();
                today_2.text = (dd - 2).ToString();
            }
        }
        else if (mm == 4 || mm == 6 || mm == 9 || mm == 11)
        {

            if (dd == 30)
            {
                today.text = dd.ToString();
                today1.text = "";
                today2.text = "";
                today_1.text = (dd - 1).ToString();
                today_2.text = (dd - 2).ToString();
            }
            else if (dd == 2)
            {
                today.text = dd.ToString();
                today1.text = (dd + 1).ToString();
                today2.text = (dd + 2).ToString();
                today_1.text = (dd - 1).ToString();
                today_2.text = "";
            }
            else if (dd == 1)
            {
                today.text = dd.ToString();
                today1.text = (dd + 1).ToString();
                today2.text = (dd + 2).ToString();
                today_1.text = "";
                today_2.text = "";
            } else if (dd == 29)
            {
                today.text = dd.ToString();
                today1.text = (dd + 1).ToString();
                today2.text = "";
                today_1.text = (dd - 1).ToString();
                today_2.text = (dd - 2).ToString();
            }
            else if (dd < 29)
            {
                today.text = dd.ToString();
                today1.text = (dd + 1).ToString();
                today2.text = (dd + 2).ToString();
                today_1.text = (dd - 1).ToString();
                today_2.text = (dd - 2).ToString();
            }

        } else if (mm == 2)
        {
            if (dd == 1)
            {
                today.text = dd.ToString();
                today1.text = (dd + 1).ToString();
                today2.text = (dd + 2).ToString();
                today_1.text = "";
                today_2.text = "";
            }else if (dd == 2)
            {
                today.text = dd.ToString();
                today1.text = (dd + 1).ToString();
                today2.text = (dd + 2).ToString();
                today_1.text =(dd - 1).ToString();
                today_2.text = "";
            }
            else if (dd == 27)
            {
                today.text = dd.ToString();
                today1.text = (dd + 1).ToString();
                today2.text = "";
                today_1.text = (dd - 1).ToString();
                today_2.text = (dd - 2).ToString();
            }else if (dd==28)
            {
                today.text = dd.ToString();
                today1.text = "";
                today2.text = "";
                today_1.text = (dd - 1).ToString();
                today_2.text = (dd - 2).ToString();
            }else if (dd>=29&&dd<=31)
            {
                dd = 28;
                today.text = dd.ToString();
                today1.text = "";
                today2.text = "";
                today_1.text = (dd - 1).ToString();
                today_2.text = (dd - 2).ToString();
            }else if (dd < 27)
            {
                today.text = dd.ToString();
                today1.text = (dd + 1).ToString();
                today2.text = (dd + 2).ToString();
                today_1.text = (dd - 1).ToString();
                today_2.text = (dd - 2).ToString();
            }
            {

            }
        }
    }
   
}
