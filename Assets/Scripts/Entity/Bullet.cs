using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Range(1, 10)]
    [SerializeField] private float speed = 10f;

    [Range(1, 10)]
    [SerializeField] private float lifeTime = 3f;

    private Rigidbody2D rigidbody;

    private void Awake() {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start() {
        Destroy(gameObject, lifeTime);
    }
    private void FixedUpdate() {
        rigidbody.velocity = transform.up * speed;
    }
    public void OnCollisionEnter2D(Collision2D collision) {
        Debug.Log("bullet disappear");
        Destroy(gameObject);
    }

}
