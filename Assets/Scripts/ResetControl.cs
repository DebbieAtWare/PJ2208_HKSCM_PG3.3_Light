using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetControl : MonoBehaviour
{
    void Start()
    {
        DestroyImmediate(GameObject.Find("IngameDebugConsole"));
        DestroyImmediate(GameObject.Find("UDPManager"));
        SceneManager.LoadScene("MainScene");
    }
}
