using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class Exports : MonoBehaviour
{
    [DllImport("__Internal")]
    public static extern void GameOver();

    [DllImport("__Internal")]
    public static extern void IncreaseScore(int score);

    [DllImport("__Internal")]
    public static extern void InitializeMultiplayer();

    [DllImport("__Internal")]
    public static extern string ReceiveLocalSettingsParams();

    [DllImport("__Internal")]
    public static extern void SendSettingsParams(string json);

    [DllImport("__Internal")]
    public static extern void StartBackgroundAnimation();

    [DllImport("__Internal")]
    public static extern void StopBackgroundAnimation();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
