using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddPicture : MonoBehaviour
{
    meyes_control my_meyes_control;

    public Texture2D texture;
    List<Color[]> pix = new List<Color[]>();
    Texture2D[] destTex;
    Rect[] rect;
    Sprite[] pic;
    public GameObject[] vuforia_obj;
    int vuforia_obj_all_obj = 12; //設定最大顯示拼圖書量
    public int row; //列
    public int column; //行
    public int sok = 0;
    public int k = 0;
    public bool picstr = false;
    public String image_path;
    public Text myText;

    public GameObject myImage;   //提示圖片

    
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
    
    
    void Start()
    {

        my_meyes_control = GameObject.Find("Manager").GetComponent<meyes_control>();

    }
    public void button_click()
    {

        for (int i = 0; i < vuforia_obj_all_obj; i++)
        {
            if (i < column * row)
            {
                vuforia_obj[i].SetActive(true);
                vuforia_obj[i].GetComponent<Transform>().position = new Vector3(0, 0, 0);
            }
            else
            {
                //Destroy(vuforia_obj[i]);
                vuforia_obj[i].SetActive(false);
            }
        }
        PickImage(512);
       // cutting(texture);
        picstr = true; //開啟判斷完成功能

    }

    public void PickImage2(string path)
    {
        for (int i = 0; i < vuforia_obj_all_obj; i++)
        {
            if (i < column * row)
            {
                vuforia_obj[i].SetActive(true);
                vuforia_obj[i].GetComponent<Transform>().position = new Vector3(0, 0, 0);
            }
            else
            {
                //Destroy(vuforia_obj[i]);
                vuforia_obj[i].SetActive(false);
            }
        }

        if (path != null)
        {
            image_path = "/storage/emulated/0/DCIM/ScreenShot/" +path;
            texture = NativeGallery.LoadImageAtPath(image_path, 512);
            cutting(texture);

        }
        picstr = true; //開啟判斷完成功能

        GameObject.Find("Manager").GetComponent<timer>().timerStr = true; //開啟計時

    }
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


    public void initialization()
    {
        for(int i = 0;i< vuforia_obj_all_obj; i++)
        {
            GameObject.Find(i.ToString()).GetComponent<SpriteRenderer>().sprite = null;
        }
    }


    public void setting_colum_row(int i)
    {
        picstr = true;
        // 相簿2X2
        if (i == 0)
        {
            column = 2;
            row = 2;
            button_click();
        }
        //相簿3X3
        else if (i == 1)
        {
            column = 3;
            row = 3;
            button_click();
        }
        //相簿3X4
        else if (i == 2)
        {
            column = 4;
            row = 3;
            button_click();
            print(row);
        }
        //相機2X2
        else if (i == 3)
        {
            column = 2;
            row = 2;
        }
        //相機3X3
        else if (i == 4)
        {
            column = 3;
            row = 3;
        }
        //相機3X4
        else if (i == 5)
        {
            column = 4;
            row = 3;
        }

    }


    public void show()
    {
        myImage.SetActive(true);
    }
    public void disappear()
    {
        myImage.SetActive(false);
    }
}
