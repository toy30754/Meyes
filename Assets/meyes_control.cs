using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Drawing;
using System.IO;

public class meyes_control : MonoBehaviour
{

    AddPicture AddData;

    public GameObject homeicon;  //回首頁按鈕
    public GameObject home;      //首頁
    public GameObject p1_1;      //拼圖1
    public GameObject p1_2_1;      //相簿拼圖2
    public GameObject p1_2_2;       //拍攝拼圖2
    public GameObject p1_3;      //拼圖3
    public GameObject p1_4;      //拼圖4

    public GameObject shot_page; //拍照頁面

    public GameObject bg;      //背景
    public GameObject bg_down; //下邊條
    public GameObject logo;

    public GameObject p2_1;      //測驗1
    public GameObject p2_2;      //測驗2
    public GameObject p2_3;      //測驗3

    public GameObject p3_1;      //紀錄1
    public GameObject p3_2;      //紀錄2
    public GameObject p3_3;      //紀錄3

    public GameObject yes1;
    public GameObject yes2;
    public GameObject no1;
    public GameObject no2;

    public Text aText;        //測試json



    public Text mText;          //顯示題目
    private int i = 0;          //計算第幾題
    private int j = 0;          //計算每題分數
    private int k = 0;          //計算總分數

    public GameObject result;
    public Sprite result1;     //儲存笑臉
    public Sprite result2;     //儲存一般臉
    public Sprite result3;     //儲存哭臉


    

    public Text myText;   //計時器時間
    public Text nText;    //完成時間

    public bool timerStr = false;  //計時器開關
    bool p1_4_show = false;
    public float temp;

    private String[] topic = { "1.判斷力上的困難：" + "\n" + "例如落入圈套或騙局、財務上不好的決定等",
                               "2.對活動和嗜好的興趣降低",
                               "3.在學習如何使用工具、設備、和小器具上有困難",
                               "4.重複相同問題、故事和陳述",
                               "5.處理複雜的財務上有困難：" + "\n" + "例如個人或家庭的收支平衡、所得稅等",
                               "6.忘記正確的月份和年份",
                               "7.有持續的思考和記憶方面的問題",
                               "8.記住約會的時間有困難" };

    public void trun_page1()
    {
        home.SetActive(false);
        p1_1.SetActive(true);
        homeicon.SetActive(true);
    }

    public void trun_page1_2_1()
    {
        p1_1.SetActive(false);
        p1_2_1.SetActive(true);
    }
    public void trun_page1_2_2()
    {
        p1_1.SetActive(false);
        p1_2_2.SetActive(true);
    }

    public void trun_page3()
    {
        bg_down.SetActive(true);
        logo.SetActive(true);
        bg.SetActive(false);
        p1_2_1.SetActive(false);
        p1_3.SetActive(true);
        temp = 0; //計時器歸0
        timerStr = true;
    }

    public void trun_shot_page()
    {
        bg_down.SetActive(false);
        logo.SetActive(false);
        bg.SetActive(false);
        p1_2_2.SetActive(false);
        shot_page.SetActive(true);
    }

    public void trun_home()
    {
        AddData.initialization();
        bg_down.SetActive(true);
        logo.SetActive(true);
        bg.SetActive(true);
        home.SetActive(true);
        p1_1.SetActive(false);
        p1_2_1.SetActive(false);
        p1_2_2.SetActive(false);
        p1_3.SetActive(false);
        shot_page.SetActive(false);
        temp = 0;                    //關閉拼圖第四頁
        p1_4.SetActive(false);
        p2_1.SetActive(false);
        p2_2.SetActive(false);
        p2_3.SetActive(false);
        i = 0;
        k = 0;
        homeicon.SetActive(false);
        timerStr = false;
        p3_1.SetActive(false);
        p3_2.SetActive(false);
        p3_3.SetActive(false);

    }


    public void trun_page2_1()
    {
        mText.text = topic[i];
        home.SetActive(false);
        p2_1.SetActive(true);
        homeicon.SetActive(true);
        j = 2;
    }

    public void trun_page2_2()
    {
        p2_2.SetActive(true);
    }

    public void select_yes()
    {
        yes1.SetActive(false);
        yes2.SetActive(true);
        no1.SetActive(true);
        no2.SetActive(false);
        p2_2.SetActive(true);
        j = 1;
    }
    public void select_no()
    {
        yes1.SetActive(true);
        yes2.SetActive(false);
        no1.SetActive(false);
        no2.SetActive(true);
        p2_2.SetActive(true);
        j = 0;
    }
    public void next_topic()
    {
        p2_2.SetActive(false);
        if (j < 2)
        {
            yes1.SetActive(true);
            yes2.SetActive(false);
            no1.SetActive(true);
            no2.SetActive(false);
            i += 1;
            if (i < 8)
            {
                mText.text = topic[i];
                k += j;
                j = 2;
            }
            if (i == 8)
            {
                k += j; //多這 一行
                p2_1.SetActive(false);
                p2_2.SetActive(false);
                print(k + "   111111");
                if (k >=  2)
                {
                    result.GetComponent<UnityEngine.UI.Image>().sprite = result2;
                }
                else
                {
                    result.GetComponent<UnityEngine.UI.Image>().sprite = result1;
                }
                p2_3.SetActive(true);

                StreamReader file = new StreamReader(System.IO.Path.Combine(Application.persistentDataPath, "test"));
                string loadJson = file.ReadToEnd();
                file.Close();
                playerState_test loadData = new playerState_test();
                loadData = JsonUtility.FromJson<playerState_test>(loadJson);
                playerState_test myPlayer1 = new playerState_test();
                myPlayer1 = loadData;
                DateTime Now = DateTime.Now;
                myPlayer1.realtime_year.Add(Now.Year);
                myPlayer1.realtime_month.Add(Now.Month);
                myPlayer1.realtime_day.Add(Now.Day);
                myPlayer1.grade.Add(k);

                string saveString = JsonUtility.ToJson(myPlayer1);
                StreamWriter file1 = new StreamWriter(System.IO.Path.Combine(Application.persistentDataPath, "test"));
                file1.Write(saveString);
                file1.Close();
            }
        }
    }

    public void trun_page3_1_1()
    {
        bg.SetActive(true);
        p1_3.SetActive(false);
        p1_4.SetActive(false);
        home.SetActive(false);
        p3_2.SetActive(true);
        homeicon.SetActive(true);
    }




    public void trun_page3_1()
    {
        home.SetActive(false);
        p3_1.SetActive(true);
        homeicon.SetActive(true);
    }

    public void trun_page3_2()
    {
        p3_1.SetActive(false);
        p3_2.SetActive(true);

    }

    public void trun_page3_3()
    {
        p3_1.SetActive(false);
        p3_3.SetActive(true);

        StreamReader file = new StreamReader(System.IO.Path.Combine(Application.persistentDataPath, "test"));
        string loadJson = file.ReadToEnd();
        file.Close();
        playerState_test loadData = new playerState_test();
        loadData = JsonUtility.FromJson<playerState_test>(loadJson); //loadData是整理過後的最終資料        
    }


    public void shot_active()
    {
        bg_down.SetActive(false);
        homeicon.SetActive(false);
    }
    public void shot_ok_active()
    {
        bg_down.SetActive(true);
        homeicon.SetActive(true);
    }

    public void save_puzzle(TimeSpan timespan,String path)
    {
        StreamReader file = new StreamReader(System.IO.Path.Combine(Application.persistentDataPath, "puzzle"));
        string loadJson = file.ReadToEnd();
        file.Close();
        playerState_puzzle loadData = new playerState_puzzle();
        loadData = JsonUtility.FromJson<playerState_puzzle>(loadJson);
        playerState_puzzle myPlayer1 = new playerState_puzzle();
        myPlayer1 = loadData;
        DateTime Now = DateTime.Now;
        myPlayer1.realtime_year.Add(Now.Year);
        myPlayer1.realtime_month.Add(Now.Month);
        myPlayer1.realtime_day.Add(Now.Day);
        myPlayer1.cost_time_hour.Add(timespan.Hours);
        myPlayer1.cost_time_min.Add(timespan.Minutes);
        myPlayer1.cost_time_sec.Add(timespan.Seconds);
        myPlayer1.image_path.Add(path);
        myPlayer1.puzzle_row.Add(AddData.row);
        myPlayer1.puzzle_column.Add(AddData.column);
        string saveString = JsonUtility.ToJson(myPlayer1);
        StreamWriter file1 = new StreamWriter(System.IO.Path.Combine(Application.persistentDataPath, "puzzle"));
        file1.Write(saveString);
        file1.Close();
    }

    void Start()
    {
        AddData = GameObject.Find("Manager").GetComponent<AddPicture>();
        print(System.IO.Path.Combine(Application.persistentDataPath, "test"));
        if (File.Exists(System.IO.Path.Combine(Application.persistentDataPath, "test")))
        {
            print("file exist");
        }else 
        {
            print("file not exist");
            playerState_test myPlayer1 = new playerState_test();
            string saveString = JsonUtility.ToJson(myPlayer1);
            StreamWriter file = new StreamWriter(System.IO.Path.Combine(Application.persistentDataPath, "test"));
            file.Write(saveString);
            file.Close();
        }
        if (File.Exists(System.IO.Path.Combine(Application.persistentDataPath, "puzzle")))
        {
            print("file exist");
        }
        else
        {
            print("file not exist");
            playerState_puzzle myPlayer1 = new playerState_puzzle();
            string saveString = JsonUtility.ToJson(myPlayer1);
            StreamWriter file = new StreamWriter(System.IO.Path.Combine(Application.persistentDataPath, "puzzle"));
            file.Write(saveString);
            file.Close();
        }

    }


    
    // Update is called once per frame
    void Update()
    {
        if (timerStr)
        {
            temp += Time.deltaTime;
            TimeSpan timeSpan = TimeSpan.FromSeconds(temp);
            myText.text = string.Format("{0:D2} : {1:D2}", timeSpan.Minutes, timeSpan.Seconds);
            //if (temp > 10) //超過一定時間，字體變紅
            //{
            //    myText.color = UnityEngine.Color.red;
            //}
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
}
