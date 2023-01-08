using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public AudioSource AudioSrc { get; set; }
    public bool IsGameOver { get; set; }
    public bool IsMultiplayer { get; set; }
    public bool IsPaused { get; set; }
    public GameObject Plane;

    // '%.3f' % 1.618 ** 3
    public static float cameraSpeed = 4.236f;

    public float CurrentPlaneLeft
    {
        get
        {
            var halfWidth = planeWidth / 2;
            return CurrentPlaneX - halfWidth;
        }
    }

    public float CurrentPlaneX;
    public static float abovePlane = 1.5f;

    // '%.3f' % 1.618 ** 3
    public static float coinRotateSpeed = 4.236f;

    public static float planeWidth = 20;
    public static float playerDownSpeed = 0.2f;

    // '%.3f' % 1.618 ** 4
    public static float playerJumpPower = 6.854f;

    public static float playerSpeed = 0.1f;
    public static float playerSquattingScale = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        AudioSrc = GetComponent<AudioSource>();
        AudioSrc.Play();
        Time.timeScale = 1;
        SendSettingsParams();
        LoadLocalSettingsParams();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void IncreaseScore(int score)
    {
        Debug.Log(score);
    }

    public void GameOver()
    {
        IsGameOver = true;
        AudioSrc.Pause();
        Time.timeScale = 0;
        Exports.GameOver();
        Debug.Log("Game over!");
    }

    public void LoadEmpty()
    {
        SceneManager.LoadScene("EmptyScene");
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void LoadMultiplayer()
    {
        SceneManager.LoadScene("MultiplayerScene");
    }

    public void LoadLocalSettingsParams()
    {
        var json = Exports.ReceiveLocalSettingsParams();
        var settingsParams = JsonUtility.FromJson<SettingsParams>(json);

        GameController.cameraSpeed = settingsParams.cameraSpeed;
        GameController.coinRotateSpeed = settingsParams.coinRotateSpeed;
        Physics.gravity = new Vector3(settingsParams.gravityX, settingsParams.gravityY, settingsParams.gravityZ);
        GameController.playerDownSpeed = settingsParams.playerDownSpeed;
        GameController.playerJumpPower = settingsParams.playerJumpPower;
        GameController.playerSpeed = settingsParams.playerSpeed;
        GameController.playerSquattingScale = settingsParams.playerSquattingScale;
    }

    public GameObject NextPlane()
    {
        var result = Instantiate(Plane, new Vector2(CurrentPlaneX + planeWidth, 0f), Quaternion.identity);
        CurrentPlaneX += planeWidth;
        return result;
    }

    public void Pause()
    {
        AudioSrc.Pause();
        Time.timeScale = 0;
        IsPaused = true;
    }

    public void Resume()
    {
        AudioSrc.Play();
        Time.timeScale = 1;
        IsPaused = false;
    }

    public static float ScreenCenter()
    {
        var left = GameController.ScreenLeft();
        var right = GameController.ScreenRight();
        return (right - left) / 2 + left;
    }

    public static float ScreenLeft()
    {
        return Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).x;
    }

    public static float ScreenRight()
    {
        return Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x;
    }

    public static float ScreenTop()
    {
        return Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height)).y;
    }

    public void SendSettingsParams()
    {
        var settingsParams = new SettingsParams();

        settingsParams.cameraSpeed = GameController.cameraSpeed;
        settingsParams.coinRotateSpeed = GameController.coinRotateSpeed;
        settingsParams.gravityX = Physics.gravity.x;
        settingsParams.gravityY = Physics.gravity.y;
        settingsParams.gravityZ = Physics.gravity.z;
        settingsParams.playerDownSpeed = GameController.playerDownSpeed;
        settingsParams.playerJumpPower = GameController.playerJumpPower;
        settingsParams.playerSpeed = GameController.playerSpeed;
        settingsParams.playerSquattingScale = GameController.playerSquattingScale;

        var json = JsonUtility.ToJson(settingsParams);
        Exports.SendSettingsParams(json);
    }

    public class SettingsParams
    {
        public float cameraSpeed;
        public float coinRotateSpeed;
        public float gravityX;
        public float gravityY;
        public float gravityZ;
        public float playerDownSpeed;
        public float playerJumpPower;
        public float playerSpeed;
        public float playerSquattingScale;
    }

    public void Toggle()
    {
        if (IsPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }
}
