using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    GameController _gameController;

    [SerializeField]
    float _distance;

    [SerializeField]
    int _min;

    [SerializeField]
    int _max;

    [SerializeField]
    float _rate;

    [SerializeField]
    float _vectorAx;

    [SerializeField]
    float _vectorAy;

    [SerializeField]
    float _vectorBx;

    [SerializeField]
    float _vectorBy;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void Spawn(GameController gameController)
    {
        var left = gameController.CurrentPlaneX + _vectorAx;
        var right = gameController.CurrentPlaneX + _vectorBx;
        var bottom = GameController.abovePlane + _vectorAy;
        var top = GameController.abovePlane + _vectorBy;

        for (var i = 0; i < Random.Range(_min, _max); i++)
        {
            if (Random.value > _rate)
            {
                Debug.Log("Rate not met.");
                continue;
            }

            for (var j = 0;; j++)
            {
                if (j > 10)
                {
                    Debug.Log("Sprite didn't spawn.");
                    break;
                }

                var hitted = !!Physics2D.OverlapBox(new Vector2(gameController.CurrentPlaneX + (_vectorAx + _vectorBx) / 2, GameController.abovePlane + (_vectorAy + _vectorBy) / 2), new Vector2(_vectorBx + _distance, _vectorBy), 0);

                if (!hitted)
                {
                    Instantiate(gameObject, new Vector2(Random.Range(left, right), Random.Range(bottom, top)), Quaternion.identity);
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

    }
}
