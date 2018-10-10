#pragma warning disable 0168 // variable declared but not used.
#pragma warning disable 0219 // variable assigned but not used.

using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Runtime.InteropServices;

public class ScreenshotManager : MonoBehaviour {
    static ScreenshotManager instnace = null;
    static GameObject go;
    static string ImagePath;
    public static event Action<Texture2D> OnScreenshotTaken;
    enum ImageType { IMAGE, SCREENSHOT };
    enum SaveStatus { NOTSAVED, SAVED, DENIED, TIMEOUT };
    public static ScreenshotManager Instance
    {
        get
        {
            go = new GameObject();
            go.name = "SDK";
            instnace = go.AddComponent<ScreenshotManager>();
            return instnace;
        }
    }
#if UNITY_IOS
    [DllImport("__Internal")]
    private static extern int GallerySave(string path);
#elif UNITY_ANDROID
    [DllImport("__Internal")]
    private static extern bool saveToGallery( string path );
#endif
    //IEnumerator 是所有非泛型列舉值的基底介面。
    public static IEnumerator Save(string fileName, Action successCallback, Action failCallbacks, string albumName = "MyScreenshots", bool callback = false )
	{
		bool photoSaved = false;
		
		string date = System.DateTime.Now.ToString("dd-MM-yy");
		
		ScreenShotNumber++;
		
		string screenshotFilename = fileName + "_" + date + "_" + ScreenShotNumber + ".png";

        Debug.Log("Save screenshot " + screenshotFilename);

#if UNITY_IOS
        
		Rect screenArea = new Rect(0, 0, Screen.width, Screen.height);
		Instance.StartCoroutine(Instance.IOSScreenshot(albumName, fileName, screenArea,Done => {
        if(Done == SaveStatus.SAVED){
            successCallback();
        }else{
            failCallbacks();
        }
        }));
        yield return new WaitForSeconds(.5f);

#elif UNITY_ANDROID

        if (Application.platform == RuntimePlatform.Android) 
			{
				Debug.Log("Android platform detected");
				
				string androidPath = "/../../../../DCIM/" + albumName + "/" + screenshotFilename;
				string path = Application.persistentDataPath + androidPath;
				string pathonly = Path.GetDirectoryName(path);
				Directory.CreateDirectory(pathonly);
				ScreenCapture.CaptureScreenshot(androidPath);
            ImagePath = androidPath;

            AndroidJavaClass obj = new AndroidJavaClass("com.ryanwebb.androidscreenshot.MainActivity");
				
				while(!photoSaved) 
				{
					photoSaved = obj.CallStatic<bool>("scanMedia", path);
               

                    yield return new WaitForSeconds(.5f);
				}
            
        } else {
		
				ScreenCapture.CaptureScreenshot(screenshotFilename);
		
			}
       

#else
			
			while(!photoSaved) 
			{
				yield return new WaitForSeconds(.5f);
		
				Debug.Log("Screenshots only available in iOS/Android mode!");
			
				photoSaved = true;
			}
		
#endif
#if UNITY_IOS

        
#elif UNITY_ANDROID
        if (callback){
        if (photoSaved)
        {
            successCallback();
        }
        else
        {
            failCallbacks();
        }
    }
#endif
        GameObject.Find("Manager").GetComponent<AddPicture>().PickImage2(screenshotFilename);
        print("photo");

    }

    IEnumerator IOSScreenshot(string albumName, string fileName, Rect screenArea, Action<SaveStatus> Done)
	{

		yield return new WaitForEndOfFrame();
#if UNITY_IOS
		Texture2D texture = new Texture2D((int)screenArea.width, (int)screenArea.height, TextureFormat.RGB24, false);
		texture.ReadPixels(screenArea, 0, 0);
		texture.Apply();

		byte[] bytes;
		bytes = texture.EncodeToPNG();
			
		if (OnScreenshotTaken != null)
			OnScreenshotTaken(texture);
		else
			Destroy(texture);

		string date = System.DateTime.Now.ToString("hh-mm-ss_dd-MM-yy");
		string screenshotFilename = fileName + "_" + date + ".png";
		string path = Application.persistentDataPath + "/" + screenshotFilename;
		Instance.StartCoroutine(Instance.IOSSave(bytes, fileName, path, ImageType.SCREENSHOT, SaveDone =>{
                    Done(SaveDone);
            }
        ));
#endif
	}

#if UNITY_IOS
	IEnumerator IOSSave(byte[] bytes, string fileName, string path, ImageType imageType, Action<SaveStatus> SaveDone)
	{
		
		int count = 0;
		SaveStatus saved = SaveStatus.NOTSAVED;

		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			System.IO.File.WriteAllBytes(path, bytes);
			while (saved == SaveStatus.NOTSAVED)
			{
				count++;
				if (count > 30)
					saved = SaveStatus.TIMEOUT;
				else
				{
                    saved = (SaveStatus)GallerySave(path);
                    saved = SaveStatus.SAVED;

                }
                yield return new WaitForSeconds(.5f);
			}
			UnityEngine.iOS.Device.SetNoBackupFlag(path);
		}

		SaveDone(saved);
	
	}
#endif
	
	public static int ScreenShotNumber 
	{
		set { PlayerPrefs.SetInt("screenShotNumber", value); }
	
		get { return PlayerPrefs.GetInt("screenShotNumber"); }
	}
}
