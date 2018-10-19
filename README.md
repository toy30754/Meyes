# Meyes
失智防衛隊

## Unity版本簡介
- Unity Editor Version: 2018.2.7f1 
- Vuforia SDK Version: 7.2.23

##  C# Script 簡介
### AddPicture
### 輸入圖片

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
### 切割圖片
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
重點於切割圖片的column行 row列與圖片的格式 Inspector中的**Read/Write Enabled** 必須打勾 
將原圖切割好存入destTex陣列後轉輸出為pic(sprite)陣列中，最後並設定好整體圖片的到大小。



