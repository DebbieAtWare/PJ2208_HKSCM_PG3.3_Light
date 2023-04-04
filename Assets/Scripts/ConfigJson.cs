using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ConfigJson : MonoBehaviour
{
    public static ConfigJson instance;

    string localJsonPath = Application.streamingAssetsPath + "/Config.json";
    string onlineJsonUrl = "https://dl.dropboxusercontent.com/s/l8ilew3994k02av/HKYAF_EducationAR_Config.json?dl=0";

    string jsonString;
    public ConfigData data;

    public delegate void OnSetupDone();
    public OnSetupDone onSetupDoneCallback;

    public Text debugText;

    void Awake()
    {
        Debug.Log("ConfigJson Awake");
        if (instance != null)
        {
            Debug.Log("More than one instance of ConfigJson");
            return;
        }
        instance = this;
    }

    public void Setup_LocalFile()
    {
        if (File.Exists(localJsonPath))
        {
            jsonString = File.ReadAllText(localJsonPath);
            data = JsonUtility.FromJson<ConfigData>(jsonString);
            if (data != null)
            {
                if (onSetupDoneCallback != null)
                {
                    onSetupDoneCallback.Invoke();
                }
            }
        }
        else
        {
            Debug.Log("Config.json not exists");
        }
    }

    public void Setup_OnlineFile()
    {
        StartCoroutine(WaitGetOnlineJson());
    }

    IEnumerator WaitGetOnlineJson()
    {
        UnityWebRequest www = UnityWebRequest.Get(onlineJsonUrl);
        yield return www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log("json error: " + www.error);
            debugText.text += "json error: " + onlineJsonUrl + "       " + www.error + "\n";
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
            debugText.text += onlineJsonUrl + "       " + www.downloadHandler.text + "\n";
            data = JsonUtility.FromJson<ConfigData>(www.downloadHandler.text);
            if (data != null)
            {
                if (onSetupDoneCallback != null)
                {
                    onSetupDoneCallback.Invoke();
                }
            }
        }
    }

    public void UpdateJsonFile()
    {
        string text = JsonUtility.ToJson(data, true);
        if (File.Exists(localJsonPath))
        {
            File.WriteAllText(localJsonPath, text);
        }
        else
        {
            Debug.Log("Config.json not exists");
        }

    }
}

[Serializable]
public class ConfigData
{
    public ConfigData_UDPSetup UDPSetup = new ConfigData_UDPSetup();
    public List<ConfigData_Cue> Cues = new List<ConfigData_Cue>();
}

[Serializable]
public class ConfigData_Cue
{
    public string ReceivedID;
    public List<ConfigData_Cue_CueStep> CueSteps = new List<ConfigData_Cue_CueStep>();
}

[Serializable]
public class ConfigData_Cue_CueStep
{
    public string SendID;
    public float Time;
}

[Serializable]
public class ConfigData_UDPSetup
{
    public string Lighting_IP;
    public int Lighting_Port;
}
