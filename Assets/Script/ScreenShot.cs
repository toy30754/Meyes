using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Vuforia;

public class ScreenShot : MonoBehaviour
{
    /// <summary>照片名稱</summary>
    public string fileName;
    /// <summary>相簿名稱</summary>
    public string albumName;
    /// <summary>拍照成功訊息文字</summary>
    //拍照成功訊息
    public string successMsg;
    /// <summary>拍照失敗訊息文字</summary>
    public string failMsg;
    /// <summary>UIManager.cs</summary>
    public UIManager UI;

    void Update()
    {
        //編輯模式
#if UNITY_EDITOR
        if (Input.GetMouseButtonUp(0))
        //手機模式
#elif UNITY_ANDROID || UNITY_IPHONE
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
#endif
        {
            //自動對焦
            CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
        }
    }
    /// <summary>拍照</summary>
    public void Shot()
    {
        //啟動拍照程序
        StartCoroutine(ScreenshotManager.Save(fileName, SuccessShot, FailShot, albumName: albumName, callback: true));

    }
    public void fb_clik()
    {

#if UNITY_IOS
           
#elif UNITY_ANDROID
        androidContext().Call("shareFb");
#endif

    }
    /// <summary>拍照成功訊息</summary>
    public void SuccessShot()
    {
        UI.ScreenShotMessage(successMsg);

    }
    /// <summary>拍照失敗訊息</summary>
    public void FailShot()
    {
        UI.ScreenShotMessage(failMsg);
        Debug.Log("拍照失敗");
    }
#if UNITY_ANDROID
    public AndroidJavaObject androidContext()
    {
        return new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
    }
#endif
}
