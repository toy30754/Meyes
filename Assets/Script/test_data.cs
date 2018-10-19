using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class test_data : MonoBehaviour {


    meyes_control testData;
    public Dropdown month_bar;
    public GameObject[] bar;
    public GameObject[] text;
    public Button next_page; 
    public Button previous_page;
    playerState_test myPlayer1 = new playerState_test();
    int page = 0;
    // Use this for initialization
    void Start () {
    }

 
    public void load_test_data()
    {
        StreamReader file = new StreamReader(System.IO.Path.Combine(Application.persistentDataPath, "test"));
        string loadJson = file.ReadToEnd();
        file.Close();
        playerState_test loadData = new playerState_test();
        loadData = JsonUtility.FromJson<playerState_test>(loadJson); //loadData是整理過後的最終資料     
        myPlayer1 = loadData;
        page = 0;
        drawPic(0);
    }

        void drawPic(int page)
    {
        if (myPlayer1.grade.Count> 7 + 7 * page)
        {
            next_page.gameObject.SetActive(true);
        }else
        {
            next_page.gameObject.SetActive(false);
        }

        if (page > 0)
        {
            previous_page.gameObject.SetActive(true);
        }else
        {
            previous_page.gameObject.SetActive(false);
        }

        for (int i = 0 ; i < 7; i++)
        {
            bar[i].GetComponent<RectTransform>().sizeDelta = new Vector2(70,0);
            text[i].GetComponent<Text>().text = "";
        }

        for (int i = 0+ 7*page; i < myPlayer1.grade.Count&& i<7+ 7*page; i++)
        {
            bar[i- 7*page].GetComponent<RectTransform>().sizeDelta = new Vector2(70, myPlayer1.grade[i]*70);
            text[i - 7 * page].GetComponent<Text>().text = myPlayer1.realtime_month[i].ToString() + "/" + myPlayer1.realtime_day[i].ToString();
        }
    }

    public class playerState_test
    {
        public List<int> grade = new List<int>();
        public List<int> realtime_year = new List<int>();
        public List<int> realtime_month = new List<int>();
        public List<int> realtime_day = new List<int>();
    }
    public class playerState_puzzle
    {
        public List<String> cost_time = new List<String>();
        public List<String> image_path = new List<String>();
        public List<int> realtime_year = new List<int>();
        public List<int> realtime_month = new List<int>();
        public List<int> realtime_day = new List<int>();

    }


    public void previous_pageBTN()
    {
        page--;
        drawPic(page);
    }
    public void next_pageBTN()
    {
        page++;
        drawPic(page);
    }

    public void month_change()
    {
        if (month_bar.value == 9)
        {
            drawPic(0);
        }
        else
        {
            for (int i = 0; i < 7; i++)
            {
                text[i].GetComponent<Text>().text = "";
                bar[i].GetComponent<RectTransform>().sizeDelta = new Vector2(70, 0);
                
            }
        }
       
    }
}
