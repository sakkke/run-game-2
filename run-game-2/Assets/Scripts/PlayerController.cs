using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator _animator;
    private AudioSource _audioSource;
    private bool _jumping;
    private Rigidbody2D _rigidbody2D;
    public AudioClip JumpAudioClip;

    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _animator = GetComponent<Animator>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
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
    }

    void OnCollisionEnter2D(Collision2D collision2D)
    {
        if (collision2D.gameObject.tag == "Plane")
        {
            _jumping = false;
        }
    }
}
