using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static float cameraSpeed = 1;
    public static float playerJumpPower = 10;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public static float ScreenLeft()
    {
        return Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).x;
    }

    public static float ScreenRight()
    {
        return Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x;
    }
}
