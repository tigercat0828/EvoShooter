using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.Search;
using UnityEngine;

public class Spitter : MonoBehaviour {
    // Here is Zombie
    public Transform target;
    public float moveSpeed;
    public float rotateSpeed = 0.0025f;
    public GameObject bulletPrefab;

    private Rigidbody2D rb;

    public float distanceToShoot =5f;
    public float distanceToStop =3f;

    public float fireRate;
    private float timeToFire;
    public Transform FirePoint;
    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        timeToFire = fireRate;
    }
    private void Update() {
        if (!target) {
            GetTarget();
        }
        else {
            RotateTowardTarget();
        }

        if(target !=null &&  Vector2.Distance(target.position, transform.position) < distanceToShoot) {
            Shoot();
        }
    }
    private void Shoot() {
        if(timeToFire < 0f) {
            var bullet =  Instantiate(bulletPrefab, FirePoint.position, FirePoint.rotation);
            timeToFire = fireRate;
        }
        else {
            timeToFire -= Time.deltaTime;
        }
    }
    private void FixedUpdate() {
        if (target != null) {
            if (Vector2.Distance(target.position, transform.position) >= distanceToStop) {
                rb.velocity = transform.up * moveSpeed;
            }
            else {
                rb.velocity = Vector2.zero;
            }
        }
       
    }
    private void GetTarget() {
        if (GameObject.FindGameObjectWithTag("Player")) {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    private void RotateTowardTarget() {
        Vector2 targetDirection = target.position - transform.position;
        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg - 90;
        Quaternion q = Quaternion.Euler(new Vector3(0, 0, angle));
        transform.localRotation = Quaternion.Slerp(transform.localRotation, q, rotateSpeed);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Player")) {
            Destroy(other.gameObject); // DoDamage
            target = null;

            // TODO : fix as health system
            HumanPlaySceneManager.manager.GameOver();
        }
        else if (other.gameObject.CompareTag("Bullet")) {
            Destroy(other.gameObject);
            Destroy(gameObject);
            HumanPlaySceneManager.manager.IncreaseScore(2);
        }
    }
}
