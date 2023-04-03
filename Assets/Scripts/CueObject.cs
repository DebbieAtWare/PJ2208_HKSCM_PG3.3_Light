using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CueObject : MonoBehaviour
{
    public string ID;
    public List<CueStepObject> CueSteps = new List<CueStepObject>();
    public int currStep = 0;

    public void Setup(ConfigData_Cue cue)
    {
        ID = cue.ID;
        for (int i = 0; i < cue.CueSteps.Count; i++)
        {
            CueStepObject obj = new CueStepObject();
            obj.ID = cue.CueSteps[i].ID;
            obj.Time = cue.CueSteps[i].Time;
            CueSteps.Add(obj);
        }
    }

    public void SendCue()
    {
        
    }
}

public class CueStepObject : MonoBehaviour
{
    public string ID;
    public float Time;

    public delegate void OnStepFinished();
    public OnStepFinished onStepFinishedCallback;

    public void SendCue()
    {
        //if ID == -1, it is a wait
        //if ID != -1, we should send the light cue
        //if time == -1, it will not resend the cue
        //if time != -1, it will repeat and resend the cue in corresponding time interval

        if (ID == "-1")
        {
            Invoke("InvokeWaitStepFinished", Time);
        }
        else
        {
            if (Time == -1)
            {
                UDPManager.instance.Send(ConfigJson.instance.data.UDPSetup.Lighting_IP, ConfigJson.instance.data.UDPSetup.Lighting_Port, ID);
            }
        }
    }

    void InvokeWaitStepFinished()
    {
        if (onStepFinishedCallback != null)
        {
            onStepFinishedCallback.Invoke();
        }
    }
}
