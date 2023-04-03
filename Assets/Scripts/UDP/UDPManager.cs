using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine.UI;

public class UDPManager : MonoBehaviour
{
    public static UDPManager instance;

    public UDPConnection connection;

    public delegate void OnReceivedMsg(string msg);
    public OnReceivedMsg onReceivedMsgCallBack;

    //for share in multiple scenes
    void Awake()
    {
        Debug.Log("UDPManager Awake");
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Update()
    {
        UpdateRun();
    }

    public void StartRun(int localPort)
    {
        connection.StartConnection(localPort);
    }

    public void UpdateRun()
    {
        foreach (var message in connection.GetMessages())
        {
            if (onReceivedMsgCallBack != null)
            {
                onReceivedMsgCallBack.Invoke(message);
            }
        }
    }

    public void Send(string sendToIp, int sendToPort, string message)
    {
        connection.Send(sendToIp, sendToPort, message);
    }

    void OnDestroy()
    {
        connection.Stop();
    }
}
