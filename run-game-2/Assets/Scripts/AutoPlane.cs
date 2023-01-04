using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoPlane : MonoBehaviour
{
    GameController _gameController;

    // Start is called before the first frame update
    void Start()
    {
        _gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        var right = GameController.ScreenRight();
        var left = GameController.ScreenLeft();

        var center = (right - left) / 2 + left;

        if (_gameController.CurrentPlaneLeft < center)
        {
            _gameController.NextPlane();
        }
    }
}
