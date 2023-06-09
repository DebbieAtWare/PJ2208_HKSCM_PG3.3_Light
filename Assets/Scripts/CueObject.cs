using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CueObject : MonoBehaviour
{
    public string receivedID;
    public List<CueStepObject> cueSteps = new List<CueStepObject>();
    public int currStep = 0;

    public void Setup(ConfigData_Cue cue)
    {
        receivedID = cue.ReceivedID;
        for (int i = 0; i < cue.CueSteps.Count; i++)
        {
            CueStepObject obj = gameObject.AddComponent<CueStepObject>();
            obj.sendID = cue.CueSteps[i].SendID;
            obj.time = cue.CueSteps[i].Time;
            cueSteps.Add(obj);
            cueSteps[i].onStepFinishedCallback += CueSteps_OnStepFinished;
        }
    }

    public void Send()
    {
        Debug.Log("Received ID: " + receivedID);
        cueSteps[0].Send();
    }

    public void ResetAll()
    {
        currStep = 0;
        for (int i = 0; i < cueSteps.Count; i++)
        {
            cueSteps[i].ResetAll();
        }
    }

    private void CueSteps_OnStepFinished()
    {
        currStep++;
        if (currStep < cueSteps.Count)
        {
            cueSteps[currStep].Send();
        }
    }

    private void OnDestroy()
    {
        for (int i = 0; i < cueSteps.Count; i++)
        {
            cueSteps[i].onStepFinishedCallback -= CueSteps_OnStepFinished;
        }
    }

}

public class CueStepObject : MonoBehaviour
{
    public string sendID;
    public float time;

    public delegate void OnStepFinished();
    public OnStepFinished onStepFinishedCallback;

    public void Send()
    {
        //if ID == -1, it is a wait
        //if ID != -1, we should send the light cue
        //if time == -1, it will not resend the cue
        //if time != -1, it will repeat and resend the cue in corresponding time interval

        if (sendID == "-1")
        {
            Debug.Log("Send Wait: " + time);
            Invoke("InvokeWaitStepFinished", time);
        }
        else
        {
            if (time == -1)
            {
                UDPManager.instance.Send(ConfigJson.instance.data.UDPSetup.Lighting_IP, ConfigJson.instance.data.UDPSetup.Lighting_Port, sendID);
                if (onStepFinishedCallback != null)
                {
                    onStepFinishedCallback.Invoke();
                }
            }
            else
            {
                InvokeRepeating("InvokeRepeatStep", 0, time);
                if (onStepFinishedCallback != null)
                {
                    onStepFinishedCallback.Invoke();
                }
            }
        }
    }

    public void ResetAll()
    {
        CancelInvoke("InvokeWaitStepFinished");
        CancelInvoke("InvokeRepeatStep");
    }

    void InvokeWaitStepFinished()
    {
        if (onStepFinishedCallback != null)
        {
            onStepFinishedCallback.Invoke();
        }
    }

    void InvokeRepeatStep()
    {
        UDPManager.instance.Send(ConfigJson.instance.data.UDPSetup.Lighting_IP, ConfigJson.instance.data.UDPSetup.Lighting_Port, sendID);
    }
}
