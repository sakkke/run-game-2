using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCircleController : MonoBehaviour
{
    MultiplayerController _multiplayerController;

    // Start is called before the first frame update
    void Start()
    {
        _multiplayerController = GameObject.FindWithTag("GameController").GetComponent<MultiplayerController>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.gameObject.tag != "Player")
        {
            return;
        }

        var client = _multiplayerController.Clients[_multiplayerController.ClientId];
        var vec = (collider2D.transform.position - client.transform.position) * 300;
        var rigidbody2D = collider2D.gameObject.GetComponent<Rigidbody2D>();
        rigidbody2D.AddForce(vec);
        Destroy(gameObject);
    }
}
