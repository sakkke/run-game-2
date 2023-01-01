using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public AudioSource AudioSrc { get; set; }
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
