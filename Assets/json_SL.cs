using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class json_SL : MonoBehaviour {

    playerState myPlayer;

    public Text myText;
    public InputField myInputField;

    // Use this for initialization
    void Start ()
    {
        myPlayer = new playerState();
    }

    public void save()
    {
        playerState myPlayer = new playerState();
        myPlayer.name = myInputField.text;
        string saveString = JsonUtility.ToJson(myPlayer);
        StreamWriter file = new StreamWriter(System.IO.Path.Combine(Application.persistentDataPath, "myPlayer"));
        file.Write(saveString);
        file.Close();
    }

    public void load()
    {
        StreamReader file = new StreamReader(System.IO.Path.Combine(Application.persistentDataPath, "myPlayer"));
        string loadJson = file.ReadToEnd();
        file.Close();
        playerState loadData = new playerState();
        loadData = JsonUtility.FromJson<playerState>(loadJson);
        myText.text = loadData.name;
    }

    public class playerState
    {
        public string name;
        public playerState() //預設建構
        {
            name = "no name";
        }
        public playerState(string n) //預設建構
        {
            name = n;
        }
    }
}
