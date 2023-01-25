using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class Exports : MonoBehaviour
{
    [DllImport("__Internal")]
    public static extern void EmitEvent(MultiplayerController.GameEventType type, string clientId);

    [DllImport("__Internal")]
    public static extern void GameOver();

    [DllImport("__Internal")]
    public static extern void IncreaseScore(int score);

    [DllImport("__Internal")]
    public static extern void InitializeGame();


    [DllImport("__Internal")]
    public static extern void InitializeMultiplayer();

    [DllImport("__Internal")]
    public static extern string ReceiveLocalSettingsParams();

    [DllImport("__Internal")]
    public static extern void SendSettingsParams(string json);

    [DllImport("__Internal")]
    public static extern void StartBackgroundAnimation();

    [DllImport("__Internal")]
    public static extern void StartScoreIncrement();

    [DllImport("__Internal")]
    public static extern void StopBackgroundAnimation();

    [DllImport("__Internal")]
    public static extern void StopScoreIncrement();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
