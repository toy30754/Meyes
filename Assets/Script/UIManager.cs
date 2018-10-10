using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    /// <summary>拍照訊息圖片</summary>
    public GameObject SSM_Panel;
    public GameObject logo;
    /// <summary>拍照訊息</summary>
    public Text SSM_text;
    void Update()
    {
        
    }
    /// <summary>拍照訊息設定</summary>
    /// <param name="msg">拍照訊息文字</param>
    public void ScreenShotMessage(string msg = "")
    {
        //設定拍照訊息
        SSM_text.text = msg;
        Debug.Log("拍照成功訊息");
        SSM_Panel.gameObject.SetActive(true);
        logo.gameObject.SetActive(true);
        // ScreenShotMessageSwitch(true);
    }
    /// <summary>拍照訊息開關</summary>
    /// <param name="switches">開啟true/關閉false</param>
    public void ScreenShotMessageSwitch(bool switches)
    {
        //開啟拍照訊息圖片
        SSM_Panel.SetActive(switches);
        logo.gameObject.SetActive(switches);
    }
}
