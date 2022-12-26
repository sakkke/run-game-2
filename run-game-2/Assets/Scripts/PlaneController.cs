using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

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
