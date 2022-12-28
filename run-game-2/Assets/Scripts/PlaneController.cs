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
            var left = _gameController.CurrentPlaneX - 15;
            var right = _gameController.CurrentPlaneX + 15;

            for (var i = 0;; i++)
            {
                if (i > 10)
                {
                    Debug.Log("Sprite didn't spawn.");
                    break;
                }

                var x = Random.Range(left, right);
                var hitted = !!Physics2D.OverlapBox(new Vector2(x, GameController.abovePlane + (GameController.ScreenTop() - GameController.abovePlane) / 2), new Vector2(4, GameController.ScreenTop() - GameController.abovePlane), 0);

                if (!hitted)
                {
                    Instantiate(Dog, new Vector2(x, GameController.abovePlane), Quaternion.identity);
                    break;
                }
                else
                {
                    Debug.Log("Expect sprite conflicts.");
                }
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
