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

    private Rigidbody2D _rb;
    private Transform _transform;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _transform = GetComponent<Transform>();
    }

    private void Update()
    {
        // On click, apply upward velocity (capped by maxAltitude)
        if (Mouse.current.leftButton.wasPressedThisFrame &&
            _transform.position.y <= _maxAltitude)
        {
            _rb.velocity = Vector2.up * _velocity;
        }
    }

    private void FixedUpdate()
    {
        // Rotate plane based on vertical speed
        float angle = _rb.velocity.y * _rotationSpeed;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameManager.Instance.GameOver();
    }
}
