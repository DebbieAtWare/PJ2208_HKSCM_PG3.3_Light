using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    [Header("Config Json")]
    public ConfigJson configJson;

    [Header("UDP")]
    public UDPManager udpManager;
    public int udp_Self_Port;

    [Header("Cue")]
    public List<CueObject> cueObjs = new List<CueObject>();

    private void Start()
    {
        configJson.onSetupDoneCallback += ConfigJson_OnSetupDone;
        configJson.Setup_LocalFile();

        udpManager.onReceivedMsgCallBack += UdpManager_OnReceivedMsg;
    }

    private void ConfigJson_OnSetupDone()
    {
        udpManager.StartRun(udp_Self_Port);

        for (int i = 0; i < cueObjs.Count; i++)
        {
            if (i < configJson.data.Cues.Count)
            {
                cueObjs[i].Setup(configJson.data.Cues[i]);
            }
        }
    }

    private void UdpManager_OnReceivedMsg(string msg)
    {
        
    }

    private void OnDestroy()
    {
        configJson.onSetupDoneCallback -= ConfigJson_OnSetupDone;
        udpManager.onReceivedMsgCallBack -= UdpManager_OnReceivedMsg;
    }
}
