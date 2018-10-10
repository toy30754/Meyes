using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class puzzle_data : MonoBehaviour {


    meyes_control testData;
    public Text row_column;
    public Texture2D texture;
    public Image data_image;
    public Dropdown month_bar;
    public GameObject[] bar;
    public GameObject[] text;
    public Button next_page;
    public Button previous_page;
    playerState_puzzle myPlayer1 = new playerState_puzzle();
    int page = 0;
    // Use this for initialization
    void Start()
    {

    }


    public void load_test_data()
    {
        StreamReader file = new StreamReader(System.IO.Path.Combine(Application.persistentDataPath, "puzzle"));
        string loadJson = file.ReadToEnd();
        file.Close();
        playerState_puzzle loadData = new playerState_puzzle();
        loadData = JsonUtility.FromJson<playerState_puzzle>(loadJson); //loadData是整理過後的最終資料     
        myPlayer1 = loadData;
        page = 0;
        drawPic(0);
    }

    void drawPic(int page)
    {
        if (myPlayer1.realtime_year.Count > 7 + 7 * page)
        {
            next_page.gameObject.SetActive(true);
        }
        else
        {
            next_page.gameObject.SetActive(false);
        }

        if (page > 0)
        {
            previous_page.gameObject.SetActive(true);
        }
        else
        {
            previous_page.gameObject.SetActive(false);
        }

        for (int i = 0; i < 7; i++)
        {
            bar[i].GetComponent<RectTransform>().sizeDelta = new Vector2(70, 0);
            bar[i].GetComponentInChildren<Text>().text = "";
            text[i].GetComponent<Text>().text = "";
        }

        for (int i = 0 + 7 * page; i < myPlayer1.realtime_year.Count && i < 7 + 7 * page; i++)
        {
            
            int x = myPlayer1.cost_time_min[i - 7 * page] * 60 + myPlayer1.cost_time_sec[i - 7 * page];
            if (x > 500)
            {
                bar[i - 7 * page].GetComponent<RectTransform>().sizeDelta = new Vector2(70, 600);
                if(myPlayer1.cost_time_sec[i - 7 * page] < 10)
                {
                    bar[i - 7 * page].GetComponentInChildren<Text>().text = myPlayer1.cost_time_min[i - 7 * page].ToString() + ":0" + myPlayer1.cost_time_sec[i - 7 * page].ToString();
                }
                else
                {
                    bar[i - 7 * page].GetComponentInChildren<Text>().text = myPlayer1.cost_time_min[i - 7 * page].ToString() + ":" + myPlayer1.cost_time_sec[i - 7 * page].ToString();
                }
               
                text[i - 7 * page].GetComponent<Text>().text = myPlayer1.realtime_month[i].ToString() + "/" + myPlayer1.realtime_day[i].ToString();
            }
            else
            {
                bar[i - 7 * page].GetComponent<RectTransform>().sizeDelta = new Vector2(70, x+100);
                if (myPlayer1.cost_time_sec[i - 7 * page] < 10)
                {
                    bar[i - 7 * page].GetComponentInChildren<Text>().text = myPlayer1.cost_time_min[i - 7 * page].ToString() + ":0" + myPlayer1.cost_time_sec[i - 7 * page].ToString();
                }
                else
                {
                    bar[i - 7 * page].GetComponentInChildren<Text>().text = myPlayer1.cost_time_min[i - 7 * page].ToString() + ":" + myPlayer1.cost_time_sec[i - 7 * page].ToString();
                }
                text[i - 7 * page].GetComponent<Text>().text = myPlayer1.realtime_month[i].ToString() + "/" + myPlayer1.realtime_day[i].ToString();
            }
            
        }
    }
    public void PickImage2(string path)
    {
        
        if (path != null)
        {
            texture = NativeGallery.LoadImageAtPath(path, 512);
            Sprite s = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
            data_image.sprite = s;
        }


    }

    public class playerState_puzzle
    {
        public List<int> cost_time_hour = new List<int>();
        public List<int> puzzle_row = new List<int>();
        public List<int> puzzle_column = new List<int>();
        public List<int> cost_time_min = new List<int>();
        public List<int> cost_time_sec = new List<int>();
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

    public void show_image(int bar)
    {
        row_column.text = myPlayer1.puzzle_row[bar + 7 * page] + "X" + myPlayer1.puzzle_column[bar + 7 * page];
        PickImage2(myPlayer1.image_path[bar +7*page]);
        data_image.gameObject.SetActive(true);
    }

    public void disappear_image()
    {
        data_image.gameObject.SetActive(false);
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
                bar[i].GetComponent<RectTransform>().sizeDelta = new Vector2(70, 0);
                bar[i].GetComponentInChildren<Text>().text = "";
                text[i].GetComponent<Text>().text = "";
            }
        }

    }
}
