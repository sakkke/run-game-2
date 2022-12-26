using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator _animator;
    private AudioSource _audioSource;
    private GameController _gameController;
    private bool _jumping;
    private Rigidbody2D _rigidbody2D;
    private SpriteRenderer _spriteRenderer;
    public AudioClip JumpAudioClip;

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
        var halfWidth = _spriteRenderer.sprite.bounds.size.x / 2;
        var PlayerLeft = GameController.ScreenLeft() + halfWidth;

        if (transform.position.x < PlayerLeft) {
            transform.position = new Vector2(PlayerLeft, transform.position.y);
        }

        if (!_jumping && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)))
        {
            _rigidbody2D.AddForce(Vector2.up * GameController.playerJumpPower, ForceMode2D.Impulse);
            _audioSource.PlayOneShot(JumpAudioClip);
            _jumping = true;
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
