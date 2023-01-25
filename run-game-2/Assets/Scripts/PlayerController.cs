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

    MultiplayerController _multiplayerController;
    Rigidbody2D _rigidbody2D;
    SpriteRenderer _spriteRenderer;
    public bool Jumping;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        _gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        if (_gameController.IsMultiplayer)
        {
            _multiplayerController = GameObject.FindWithTag("GameController").GetComponent<MultiplayerController>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_gameController.IsGameOver)
        {
            return;
        }

        if (!_gameController.IsMultiplayer && Input.GetKeyDown(KeyCode.Escape))
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

        if (_gameController.IsMultiplayer || !(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.H)))
        {
            transform.position += Vector3.right * Time.deltaTime * GameController.cameraSpeed;
        }

        if (transform.position.x < PlayerLeft) {
            transform.position = new Vector2(PlayerLeft, transform.position.y);
        }

        if (transform.position.x > PlayerRight) {
            transform.position = new Vector2(PlayerRight, transform.position.y);
        }

        if (!_gameController.IsMultiplayer && !Jumping && (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.K) || Input.GetMouseButtonDown(0)))
        {
            Jump();
        }

        if (_gameController.IsMultiplayer && !Jumping && (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.K) || Input.GetMouseButtonDown(0)))
        {
            _multiplayerController.EmitEvent(MultiplayerController.GameEventType.Jump, _multiplayerController.ClientId);
        }

        if (!_gameController.IsMultiplayer && Jumping && (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.J)))
        {
            Dive();
        }

        if (_gameController.IsMultiplayer && Jumping && (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.J)))
        {
            _multiplayerController.EmitEvent(MultiplayerController.GameEventType.Dive, _multiplayerController.ClientId);
        }

        if (!_gameController.IsMultiplayer && !Jumping && (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.J)))
        {
            Squat();
        }
        else if (!_gameController.IsMultiplayer && !Jumping)
        {
            StandUp();
        }

        if (_gameController.IsMultiplayer && !Jumping && (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.J)))
        {
            _multiplayerController.EmitEvent(MultiplayerController.GameEventType.Squat, _multiplayerController.ClientId);
        }
        else if (_gameController.IsMultiplayer && !Jumping)
        {
            _multiplayerController.EmitEvent(MultiplayerController.GameEventType.StandUp, _multiplayerController.ClientId);
        }

        if (!_gameController.IsMultiplayer && Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.H))
        {
            MoveLeft();
        }
        else if (!_gameController.IsMultiplayer)
        {
            MoveBreak();
        }

        if (_gameController.IsMultiplayer && Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.H))
        {
            _multiplayerController.EmitEvent(MultiplayerController.GameEventType.MoveLeft, _multiplayerController.ClientId);
        }
        else if (_gameController.IsMultiplayer)
        {
            _multiplayerController.EmitEvent(MultiplayerController.GameEventType.MoveBreak, _multiplayerController.ClientId);
        }

        if (!_gameController.IsMultiplayer && Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.L))
        {
            MoveRight();
        }
        else if (!_gameController.IsMultiplayer)
        {
            MoveBreak();
        }

        if (_gameController.IsMultiplayer && Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.L))
        {
            _multiplayerController.EmitEvent(MultiplayerController.GameEventType.MoveRight, _multiplayerController.ClientId);
        }
        else if (_gameController.IsMultiplayer)
        {
            _multiplayerController.EmitEvent(MultiplayerController.GameEventType.MoveBreak, _multiplayerController.ClientId);
        }

        if (Jumping)
        {
            _animator.SetBool("Jump", true);
        }
        else
        {
            _animator.SetBool("Jump", false);
        }
    }

    public void Dive()
    {
        var vec = Vector2.down * GameController.playerDownSpeed;
        _rigidbody2D.AddForce(vec);
    }

    public void Jump()
    {
        var vec = Vector2.up * GameController.playerJumpPower;
        _rigidbody2D.AddForce(vec, ForceMode2D.Impulse);
        _audioSource.PlayOneShot(_jumpAudioClip);
        Jumping = true;
    }

    public void MoveBreak()
    {
        var vec = new Vector2(0, _rigidbody2D.velocity.y);
        _rigidbody2D.velocity = vec;
    }

    public void MoveLeft()
    {
        var vec = Vector2.left * (GameController.playerSpeed - _rigidbody2D.velocity.x) * GameController.playerSpeed;
        _rigidbody2D.AddForce(vec);
    }

    public void MoveRight()
    {
        var vec = Vector2.right * (GameController.playerSpeed - _rigidbody2D.velocity.x) * GameController.playerSpeed;
        _rigidbody2D.AddForce(vec);
    }

    void OnCollisionEnter2D(Collision2D collision2D)
    {
        if (collision2D.gameObject.tag == "Plane")
        {
            Jumping = false;
        }
    }

    public void Squat()
    {
        transform.localScale = new Vector2(transform.localScale.x, GameController.playerSquattingScale);
    }

    public void StandUp()
    {
        transform.localScale = new Vector2(transform.localScale.x, 1);
    }
}
