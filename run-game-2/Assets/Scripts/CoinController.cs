using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    GameController _gameController;

    [SerializeField]
    AudioClip _triggerAudioClip;

    // Start is called before the first frame update
    void Start()
    {
        _gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_gameController.IsPaused)
        {
            return;
        }

        transform.Rotate(0, GameController.coinRotateSpeed, 0);
    }

    void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.gameObject.tag == "Player")
        {
            Destroy(gameObject);
            _gameController.IncreaseScore(1000);
            _gameController.AudioSrc.PlayOneShot(_triggerAudioClip);
        }
    }
}
