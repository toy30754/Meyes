using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Drawing;


public class showimage : MonoBehaviour {

    public GameObject mimage;
    public void show()
    {               
        mimage.SetActive(true);
    }
    
    public void dis()
    {
        mimage.SetActive(false);
    }

    void Start ()
    {
        Bitmap image = new Bitmap("C:/Users/Jeff/Desktop/shape/1.jpg");

        Texture2D t = new Texture2D(image.Width, image.Height);

        for (int x = 0; x < image.Width; x++)
        {
            for (int y = 0; y < image.Height; y++)
            {
                t.SetPixel(x, y,
                      new Color32(
                       image.GetPixel(x, y).R,
                       image.GetPixel(x, y).G,
                       image.GetPixel(x, y).B,
                       image.GetPixel(x, y).A
                       )
                );
            }
        }
        t.Apply();

        Sprite s = Sprite.Create(t, new Rect(0, 0, t.width, t.height), Vector2.zero);
        mimage.GetComponent<UnityEngine.UI.Image>().sprite = s;
    }
	
	void Update ()
    {
		
	}
}
