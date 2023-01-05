using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerController : MonoBehaviour
{
    [SerializeField]
    GameObject _player;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CreateClient(string clientId)
    {
        var vec = new Vector2(GameController.ScreenCenter(), 5);
        Instantiate(_player, vec, Quaternion.identity);
    }
}
