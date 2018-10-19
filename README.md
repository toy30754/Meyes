# Meyes
失智防衛隊

## Unity版本簡介
- Unity Editor Version: 2018.2.7f1 
- Vuforia SDK Version: 7.2.23

##  C# Script 簡介
### AddPicture.cs
讀取圖片 切割圖片 檢查拼圖是否完成
#### 輸入圖片
使用套件**NativeGallery**\
套件來源:https://github.com/yasirkula/UnityNativeGallery \
其中NativeGallery.cs需修改**Read/Write Enabled**
```
/*更改此區markTextureNonReadable為false*/
    public static Texture2D LoadImageAtPath( string imagePath, int maxSize = -1, bool markTextureNonReadable = false,
		bool generateMipmaps = true, bool linearColorSpace = false )
```
```C#
 private void PickImage(int maxSize)
    {
        //找尋路徑
        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
        {
            if (path != null)
            {
                // Create Texture from selected image 目前尚未研究maxSize值得變化，目前設定都為512
                texture = NativeGallery.LoadImageAtPath(path, maxSize);
                //紀錄圖片的路徑，存到DataBase
                image_path = path;
                //找不到圖片
                if (texture == null)
                {
                    Debug.Log("Couldn't load texture from " + path);
                    return;
                }
                //進行切割圖片
                cutting(texture);

            }
        }, "Select a PNG image", "image/png", maxSize);

        Debug.Log("Permission result: " + permission);
    }
```

#### 切割圖片
重點於切割圖片的column行 row列與圖片的格式 Inspector中的**Read/Write Enabled** 必須打勾\
將原圖切割好存入destTex陣列後轉輸出為pic(sprite)陣列中，最後並設定好整體圖片的到大小。

```C#
    public void cutting(Texture2D sourceTex)
    {
        //初始化
        int width = Mathf.FloorToInt(sourceTex.width) / column;
        int height = Mathf.FloorToInt(sourceTex.height) / row;
        destTex = new Texture2D[width * height];
        rect = new Rect[width * height];
        pic = new Sprite[width * height];
        myImage.GetComponent<UnityEngine.UI.Image>().sprite = Sprite.Create(sourceTex,new Rect(0, 0, sourceTex.width, sourceTex.height), Vector2.zero);
        //清除站存檔
        pix.Clear();
        for (int i = 0; i < row * column; i++)
        {
            //切割圖片 j判斷第幾層 i判斷這層的第幾個
            int j = i / column;
            pix.Add(sourceTex.GetPixels(width * (i - j * column), j * height, width, height));
            //複製圖片
            destTex[i] = new Texture2D(width, height);
            destTex[i].SetPixels(pix[i]);
            destTex[i].Apply();
            rect[i] = new Rect(0, 0, destTex[i].width, destTex[i].height);
            pic[i] = Sprite.Create(destTex[i], rect[i], Vector2.zero);
            //vuforia 子物件(底下的要出現的東西)
            GameObject.Find(i.ToString()).GetComponent<SpriteRenderer>().sprite = pic[i];
            print(pic[i].rect.width + "  " + pic[i].rect.height);
            GameObject.Find(i.ToString()).GetComponent<Transform>().localScale = new Vector3(138.219318f / pic[i].rect.width, 136.47168f / pic[i].rect.height, 1);
        }
    }
```

#### 檢查拼圖是否正確

```C#
void Update()
    {
        if (picstr)
        {
            //判斷錯誤數量
            sok = 0;
            //顯示出幾張圖片
            k = 0;
            for (int i = 0; i < row * column; i++)
            {
                if (GameObject.Find(i.ToString()).GetComponent<SpriteRenderer>().enabled == true)
                {
                    k++;
                }
            }
            //判斷x軸，每張圖片的x點的大小比較
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < column - 1; j++)
                {
                    if (GameObject.Find((column * i + j).ToString()).GetComponentInParent<Transform>().position.x < GameObject.Find((column * i + j + 1).ToString()).GetComponentInParent<Transform>().position.x)
                    {

                    }
                    else
                    {
                        sok += 1;
                    }
                }
            }
            //判斷y軸，每張圖片的y點的大小比較
            for (int i = 0; i < column; i++)
            {
                for (int j = 0; j < row - 1; j++)
                {
                    if (GameObject.Find((i + j * row).ToString()).GetComponentInParent<Transform>().position.y < GameObject.Find((i + (j + 1) * row).ToString()).GetComponentInParent<Transform>().position.y)
                    {

                    }
                    else
                    {
                        sok += 1;
                    }
                }
            }
            if (sok == 0 && k == row* column)
            {
                //計時器關閉
                my_meyes_control.timerStr = false;
                TimeSpan timeSpan = TimeSpan.FromSeconds(my_meyes_control.temp);
                my_meyes_control.nText.text = string.Format("共花{0:D2}分{1:D2}秒", timeSpan.Minutes, timeSpan.Seconds);
                my_meyes_control.p1_4.SetActive(true);
                picstr = false;
                //完成 存入dataBase
                my_meyes_control.save_puzzle(timeSpan, image_path);

            }
            else
            {
                my_meyes_control.p1_4.SetActive(false);
            }

        }
    }
```
### meyes_control.cs
版控 計時器 寫入DataBase 心智測驗
#### 心智測驗
```C#
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
                //寫入dataBase
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
```

#### DataBase寫入json
在Start時需先檢測是否有此json，否則創建。
```C#
//拼圖測驗
 if (File.Exists(System.IO.Path.Combine(Application.persistentDataPath, "puzzle")))
        {
            print("file exist");
        }else 
        {
            print("file not exist");
            playerState_test myPlayer1 = new playerState_test();
            string saveString = JsonUtility.ToJson(myPlayer1);
            StreamWriter file = new StreamWriter(System.IO.Path.Combine(Application.persistentDataPath, "puzzle"));
            file.Write(saveString);
            file.Close();
        }
```
創建DataBase格式(json格式)
```C#
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
```
開檔讀檔 寫入

```C#
  public void save_puzzle(TimeSpan timespan,String path)
    {
        //開啟檔案位置
        StreamReader file = new StreamReader(System.IO.Path.Combine(Application.persistentDataPath, "puzzle"));
        //讀取檔案
        string loadJson = file.ReadToEnd();
        file.Close();
        playerState_puzzle loadData = new playerState_puzzle();
        //將string 轉為 Json格式
        loadData = JsonUtility.FromJson<playerState_puzzle>(loadJson);
        playerState_puzzle myPlayer1 = new playerState_puzzle();
        myPlayer1 = loadData;
        DateTime Now = DateTime.Now;
        //使用 . Add功能加入
        myPlayer1.realtime_year.Add(Now.Year);
        myPlayer1.realtime_month.Add(Now.Month);
        myPlayer1.realtime_day.Add(Now.Day);
        myPlayer1.cost_time_hour.Add(timespan.Hours);
        myPlayer1.cost_time_min.Add(timespan.Minutes);
        myPlayer1.cost_time_sec.Add(timespan.Seconds);
        myPlayer1.image_path.Add(path);
        myPlayer1.puzzle_row.Add(AddData.row);
        myPlayer1.puzzle_column.Add(AddData.column);
        //將Json 轉為 string
        string saveString = JsonUtility.ToJson(myPlayer1);
        //寫入檔案
        StreamWriter file1 = new StreamWriter(System.IO.Path.Combine(Application.persistentDataPath, "puzzle"));
        file1.Write(saveString);
        file1.Close();
    }
```

### test_data.cs 與 puzzle_data.cs 畫直方圖
重新讀取資料後繪製直方圖\
使用sizeDelta\
鎖定物件中心點為最地部，調整Height
```C#
 bar[i- 7*page].GetComponent<RectTransform>().sizeDelta = new Vector2(70, myPlayer1.grade[i]*70);
```


### Vuforia_focus.cs
用來對焦Vuforia Camera，放置於ARCamera底下
```C#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class Vuforia_focus : MonoBehaviour
{

    void Start()
    {

        VuforiaARController.Instance.RegisterVuforiaStartedCallback(OnVuforiaStarted);
        VuforiaARController.Instance.RegisterOnPauseCallback(OnPaused);
    }

    void Update()
    {

    }

    private void OnVuforiaStarted()
    {
        CameraDevice.Instance.SetFocusMode(
            CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
    }

    private void OnPaused(bool paused)
    {
        if (!paused)
        { // resumed
            // Set again autofocus mode when app is resumed
            CameraDevice.Instance.SetFocusMode(
                CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
        }
    }

}
```

