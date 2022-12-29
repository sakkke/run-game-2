using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Animator _animator;
    AudioSource _audioSource;
    GameController _gameController;

    [SerializeField]
    AudioClip _jumpAudioClip;

    bool _jumping;
    Rigidbody2D _rigidbody2D;
    SpriteRenderer _spriteRenderer;


    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        _gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _gameController.Toggle();
        }

        if (_gameController.IsPaused)
        {
            return;
        }

        var halfWidth = _spriteRenderer.sprite.bounds.size.x / 2;
        var PlayerLeft = GameController.ScreenLeft() + halfWidth;
        var PlayerRight = GameController.ScreenRight() - halfWidth;

        if (!(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.H)))
        {
            transform.position += Vector3.right * Time.deltaTime * GameController.cameraSpeed;
        }

        if (transform.position.x < PlayerLeft) {
            transform.position = new Vector2(PlayerLeft, transform.position.y);
        }

        if (transform.position.x > PlayerRight) {
            transform.position = new Vector2(PlayerRight, transform.position.y);
        }

        if (!_jumping && (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.K) || Input.GetMouseButtonDown(0)))
        {
            _rigidbody2D.AddForce(Vector2.up * GameController.playerJumpPower, ForceMode2D.Impulse);
            _audioSource.PlayOneShot(_jumpAudioClip);
            _jumping = true;
        }

        if (_jumping && (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.J)))
        {
            transform.position = new Vector2(transform.position.x, transform.position.y - GameController.playerDownSpeed);
        }

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.H))
        {
            transform.position = new Vector2(transform.position.x - GameController.playerSpeed, transform.position.y);
        }

        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.L))
        {
            transform.position = new Vector2(transform.position.x + GameController.playerSpeed, transform.position.y);
        }

        if (_jumping)
        {
            _animator.SetBool("Jump", true);
        }
        else
        {
            _animator.SetBool("Jump", false);
        }

        if (_gameController.CurrentPlaneLeft < transform.position.x)
        {
            _gameController.NextPlane();
        }
    }

    void OnCollisionEnter2D(Collision2D collision2D)
    {
        if (collision2D.gameObject.tag == "Plane")
        {
            _jumping = false;
        }
    }
}
