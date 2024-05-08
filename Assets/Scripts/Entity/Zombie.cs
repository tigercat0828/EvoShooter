using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.Search;
using UnityEngine;

public class Zombie : MonoBehaviour, IEntity {
    // Zombie can aware the agent in any distance
    public Transform target;
    private Rigidbody2D rb;
    public int HealtPoint { get; set; } = 100;
    public float MoveSpeed { get; set; } = 3f;
    public float RotateSpeed { get; set; } = 0.05f;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update() {
        if (!target) {
            GetTarget();
        }
        else {
            RotateTowardTarget();
        }
    }
    private void FixedUpdate() {
        rb.velocity = transform.up * MoveSpeed;
    }
    private void GetTarget() {
        if (GameObject.FindGameObjectWithTag("Player")) {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }
    private void RotateTowardTarget() {
        Vector2 targetDirection = target.position - transform.position;
        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg -90;
        Quaternion q = Quaternion.Euler(new Vector3(0, 0, angle));
        transform.localRotation = Quaternion.Slerp(transform.localRotation, q, RotateSpeed);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Player")) {
            Destroy(other.gameObject); // DoDamage
            target = null;
        }else if (other.gameObject.CompareTag("Bullet") ){
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }

    public void Attack(int damage) {
        throw new System.NotImplementedException();
    }

    public void TakeDamage(int amount) {
        throw new System.NotImplementedException();
    }
}
