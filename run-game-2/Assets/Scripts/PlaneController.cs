using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneController : MonoBehaviour
{
    [SerializeField]
    bool _canSpawn;

    GameController _gameController;

    [SerializeField]
    Barrier[] _barriers;

    // Start is called before the first frame update
    void Start()
    {
        _gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();

        if (_canSpawn)
        {
            foreach (var barrier in _barriers)
            {
                barrier.Spawn(_gameController);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        var halfWidth = GameController.planeWidth / 2;

        if (transform.position.x + halfWidth < GameController.ScreenLeft())
        {
            Destroy(gameObject);
        }
    }
}
