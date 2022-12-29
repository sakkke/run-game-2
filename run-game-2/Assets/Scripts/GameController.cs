using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
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
    public static float playerSpeed = 0.1f;

    // '%.3f' % 1.618 ** 4
    public static float playerJumpPower = 6.854f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GameOver()
    {
        Debug.Log("Game over!");
    }

    public GameObject NextPlane()
    {
        var result = Instantiate(Plane, new Vector2(CurrentPlaneX + planeWidth, 0f), Quaternion.identity);
        CurrentPlaneX += planeWidth;
        return result;
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
}
