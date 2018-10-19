# Meyes
�������ö�

## Unity����²��
- Unity Editor Version: 2018.2.7f1 
- Vuforia SDK Version: 7.2.23

##  C# Script ²��
### AddPicture.cs
Ū���Ϥ� ���ιϤ� �ˬd���ϬO�_����
#### ��J�Ϥ�
�ϥήM��**NativeGallery**\
�M��ӷ�:https://github.com/yasirkula/UnityNativeGallery \
�䤤NativeGallery.cs�ݭק�**Read/Write Enabled**
```
/*��惡��markTextureNonReadable��false*/
    public static Texture2D LoadImageAtPath( string imagePath, int maxSize = -1, bool markTextureNonReadable = false,
		bool generateMipmaps = true, bool linearColorSpace = false )
```
```C#
 private void PickImage(int maxSize)
    {
        //��M���|
        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
        {
            if (path != null)
            {
                // Create Texture from selected image �ثe�|����smaxSize�ȱo�ܤơA�ثe�]�w����512
                texture = NativeGallery.LoadImageAtPath(path, maxSize);
                //�����Ϥ������|�A�s��DataBase
                image_path = path;
                //�䤣��Ϥ�
                if (texture == null)
                {
                    Debug.Log("Couldn't load texture from " + path);
                    return;
                }
                //�i����ιϤ�
                cutting(texture);

            }
        }, "Select a PNG image", "image/png", maxSize);

        Debug.Log("Permission result: " + permission);
    }
```

#### ���ιϤ�
���I����ιϤ���column�� row�C�P�Ϥ����榡 Inspector����**Read/Write Enabled** ��������\
�N��Ϥ��Φn�s�JdestTex�}�C�����X��pic(sprite)�}�C���A�̫�ó]�w�n����Ϥ�����j�p�C

```C#
    public void cutting(Texture2D sourceTex)
    {
        //��l��
        int width = Mathf.FloorToInt(sourceTex.width) / column;
        int height = Mathf.FloorToInt(sourceTex.height) / row;
        destTex = new Texture2D[width * height];
        rect = new Rect[width * height];
        pic = new Sprite[width * height];
        myImage.GetComponent<UnityEngine.UI.Image>().sprite = Sprite.Create(sourceTex,new Rect(0, 0, sourceTex.width, sourceTex.height), Vector2.zero);
        //�M�����s��
        pix.Clear();
        for (int i = 0; i < row * column; i++)
        {
            //���ιϤ� j�P�_�ĴX�h i�P�_�o�h���ĴX��
            int j = i / column;
            pix.Add(sourceTex.GetPixels(width * (i - j * column), j * height, width, height));
            //�ƻs�Ϥ�
            destTex[i] = new Texture2D(width, height);
            destTex[i].SetPixels(pix[i]);
            destTex[i].Apply();
            rect[i] = new Rect(0, 0, destTex[i].width, destTex[i].height);
            pic[i] = Sprite.Create(destTex[i], rect[i], Vector2.zero);
            //vuforia �l����(���U���n�X�{���F��)
            GameObject.Find(i.ToString()).GetComponent<SpriteRenderer>().sprite = pic[i];
            print(pic[i].rect.width + "  " + pic[i].rect.height);
            GameObject.Find(i.ToString()).GetComponent<Transform>().localScale = new Vector3(138.219318f / pic[i].rect.width, 136.47168f / pic[i].rect.height, 1);
        }
    }
```

#### �ˬd���ϬO�_���T

```C#
void Update()
    {
        if (picstr)
        {
            //�P�_���~�ƶq
            sok = 0;
            //��ܥX�X�i�Ϥ�
            k = 0;
            for (int i = 0; i < row * column; i++)
            {
                if (GameObject.Find(i.ToString()).GetComponent<SpriteRenderer>().enabled == true)
                {
                    k++;
                }
            }
            //�P�_x�b�A�C�i�Ϥ���x�I���j�p���
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
            //�P�_y�b�A�C�i�Ϥ���y�I���j�p���
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
                //�p�ɾ�����
                my_meyes_control.timerStr = false;
                TimeSpan timeSpan = TimeSpan.FromSeconds(my_meyes_control.temp);
                my_meyes_control.nText.text = string.Format("�@��{0:D2}��{1:D2}��", timeSpan.Minutes, timeSpan.Seconds);
                my_meyes_control.p1_4.SetActive(true);
                picstr = false;
                //���� �s�JdataBase
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
���� �p�ɾ� �g�JDataBase �ߴ�����
#### �ߴ�����
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
                k += j; //�h�o �@��
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
                //�g�JdataBase
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

#### DataBase�g�Jjson
�bStart�ɻݥ��˴��O�_����json�A�_�h�ЫءC
```C#
//���ϴ���
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
�Ы�DataBase�榡(json�榡)
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
�}��Ū�� �g�J

```C#
  public void save_puzzle(TimeSpan timespan,String path)
    {
        //�}���ɮצ�m
        StreamReader file = new StreamReader(System.IO.Path.Combine(Application.persistentDataPath, "puzzle"));
        //Ū���ɮ�
        string loadJson = file.ReadToEnd();
        file.Close();
        playerState_puzzle loadData = new playerState_puzzle();
        //�Nstring �ର Json�榡
        loadData = JsonUtility.FromJson<playerState_puzzle>(loadJson);
        playerState_puzzle myPlayer1 = new playerState_puzzle();
        myPlayer1 = loadData;
        DateTime Now = DateTime.Now;
        //�ϥ� . Add�\��[�J
        myPlayer1.realtime_year.Add(Now.Year);
        myPlayer1.realtime_month.Add(Now.Month);
        myPlayer1.realtime_day.Add(Now.Day);
        myPlayer1.cost_time_hour.Add(timespan.Hours);
        myPlayer1.cost_time_min.Add(timespan.Minutes);
        myPlayer1.cost_time_sec.Add(timespan.Seconds);
        myPlayer1.image_path.Add(path);
        myPlayer1.puzzle_row.Add(AddData.row);
        myPlayer1.puzzle_column.Add(AddData.column);
        //�NJson �ର string
        string saveString = JsonUtility.ToJson(myPlayer1);
        //�g�J�ɮ�
        StreamWriter file1 = new StreamWriter(System.IO.Path.Combine(Application.persistentDataPath, "puzzle"));
        file1.Write(saveString);
        file1.Close();
    }
```

### test_data.cs �P puzzle_data.cs �e�����
���sŪ����ƫ�ø�s�����\
�ϥ�sizeDelta\
��w���󤤤��I���̦a���A�վ�Height
```C#
 bar[i- 7*page].GetComponent<RectTransform>().sizeDelta = new Vector2(70, myPlayer1.grade[i]*70);
```


### Vuforia_focus.cs
�Ψӹ�JVuforia Camera�A��m��ARCamera���U
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

