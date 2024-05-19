using UnityEngine;

public class Spitter : MonoBehaviour {
    // Here is Zombie
    public float MoveSpeed =1;
    public float RotateSpeed = 0.0025f;
    public float FireRate =1;
    public float ViewDistance  = 5;

    public float distanceToShoot = 5f;
    public float distanceToStop = 3f;

    [SerializeField] private Transform target;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform FirePoint;
    private float fireInterval;
    private float fireTimer;
    private Rigidbody2D rb;
    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        fireInterval = 1 / FireRate;
    }
    private void Update() {
        if (!target) {
            GetTarget();
        }
        else {
            RotateTowardTarget();
        }

        if (target != null && Vector2.Distance(target.position, transform.position) < distanceToShoot) {
            Shoot();
        }
    }
    private void Shoot() {
        if (fireTimer < 0f) {
            var bullet = Instantiate(bulletPrefab, FirePoint.position, FirePoint.rotation);
            fireTimer = fireInterval;
        }
        else {
            fireTimer -= Time.deltaTime;
        }
    }
    private void FixedUpdate() {
        if (target != null) {
            if (Vector2.Distance(target.position, transform.position) >= distanceToStop) {
                rb.velocity = transform.up * MoveSpeed;
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
        transform.localRotation = Quaternion.Slerp(transform.localRotation, q, RotateSpeed);
    }

}
