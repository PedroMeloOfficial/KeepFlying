using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FlyBehavior : MonoBehaviour
{
    [Header("Flight Settings")]
    [SerializeField] private float _velocity = 1.5f;
    [SerializeField] private float _rotationSpeed = 10f;
    [SerializeField] private float _maxAltitude = 1.1f;

    [Header("References")]
    [SerializeField] private Animator _animator;      // your Animator
    [SerializeField] private AudioSource _audioSource; // AudioSource component

    [Header("SFX Clips")]
    [SerializeField] private AudioClip _jumpSfx;      // jump sound
    [SerializeField] private AudioClip _crumpleSfx;   // crumple sound

    private Rigidbody2D _rb;
    private Transform _transform;
    private bool _gameOver;

    private void Start()
    {
        _gameOver = false;
        _rb = GetComponent<Rigidbody2D>();
        _transform = transform;

        if (_animator == null)
            _animator = GetComponent<Animator>();

        if (_audioSource == null)
            _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame || Input.GetKeyDown(KeyCode.Space) &&
            _transform.position.y <= _maxAltitude && _gameOver == false)
        {
            // 1) apply upward velocity
            _rb.velocity = Vector2.up * _velocity;

            // 2) play jump SFX
            if (_jumpSfx != null)
                _audioSource.PlayOneShot(_jumpSfx);
        }
    }

    private void FixedUpdate()
    {
        float angle = _rb.velocity.y * _rotationSpeed;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    private IEnumerator HandleCrash()
    {
        _gameOver = true;

        // 1) trigger crumple anim
        _animator.SetTrigger("Crumpled");

        // 2) play crumple SFX
        if (_crumpleSfx != null)
            _audioSource.PlayOneShot(_crumpleSfx);

        // 3) stop physics
        _rb.simulated = false;

        // 4) wait a bit, then end game
        yield return new WaitForSeconds(0.05f);
        GameManager.Instance.GameOver();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        StartCoroutine(HandleCrash());
    }
}
