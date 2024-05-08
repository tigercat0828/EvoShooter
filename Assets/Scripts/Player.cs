using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;

    [SerializeField] private float _fireRate = 0.1f;
    
    [SerializeField] private GameObject bulletPrefab;

    [SerializeField] private Transform gunBarrel;

    private Rigidbody2D _rigidbody;
    
    private float mx;
    private float my;
    private Vector2 mousePos;
    private float fireTimer;


    private void Awake() {
        _rigidbody = GetComponent<Rigidbody2D>();
    }
    private void Update() {
        mx = Input.GetAxisRaw("Horizontal");
        my = Input.GetAxisRaw("Vertical");
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        float angle = Mathf.Atan2(mousePos.y - transform.position.y, mousePos.x - transform.position.x) * Mathf.Rad2Deg - 90f;
        transform.localRotation = Quaternion.Euler(0, 0, angle);
        if (Input.GetMouseButton(0) && fireTimer < 0f) {
            Shoot();
            fireTimer = _fireRate;
        }
        else {
            fireTimer -= Time.deltaTime;
        }
    }
    private void FixedUpdate() {
        _rigidbody.velocity = new Vector2(mx, my).normalized * _moveSpeed;
    }

    // Action Behaviour
    private void Shoot() {
        Instantiate(bulletPrefab, gunBarrel.position, gunBarrel.rotation);
    }
    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("EnemyBullet")) {
            Destroy(gameObject);
        }
    }
}
