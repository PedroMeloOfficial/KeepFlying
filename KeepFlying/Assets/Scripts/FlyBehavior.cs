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
    [SerializeField] private Animator _animator;     // Drag your Animator here in the Inspector

    private Rigidbody2D _rb;
    private Transform _transform;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _transform = transform;

        // If you didn¡¯t hook it up in the Inspector, try to grab it at runtime:
        if (_animator == null)
        {
            _animator = GetComponent<Animator>();
            print("Got animator");
        }
            
    }

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame &&
            _transform.position.y <= _maxAltitude)
        {
            _rb.velocity = Vector2.up * _velocity;
        }
    }

    private void FixedUpdate()
    {
        float angle = _rb.velocity.y * _rotationSpeed;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    private IEnumerator HandleCrash()
    {
        _animator.SetTrigger("Crumpled");
        _rb.simulated = false;
        // Wait for the length of the crumple clip:
        yield return new WaitForSeconds(0.05f);
        GameManager.Instance.GameOver();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        StartCoroutine(HandleCrash());
    }

}
