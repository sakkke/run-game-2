using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableMultiplayer : MonoBehaviour
{
    GameController _gameController;

    // Start is called before the first frame update
    void Start()
    {
        _gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        _gameController.IsMultiplayer = true;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
