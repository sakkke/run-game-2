using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneController : MonoBehaviour
{
    private GameController _gameController;
    public bool CanSpawn;
    public GameObject Dog;

    // Start is called before the first frame update
    void Start()
    {
        _gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();

        if (CanSpawn)
        {
            var left = _gameController.CurrentPlaneX - 10;
            var right = _gameController.CurrentPlaneX + 10;
            var x = Random.Range(left, right);
            Instantiate(Dog, new Vector2(x, GameController.abovePlane), Quaternion.identity);
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
