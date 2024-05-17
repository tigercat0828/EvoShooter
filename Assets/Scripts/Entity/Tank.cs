using UnityEngine;

public class Tank : MonoBehaviour {
    // high health point, slow movement speed
    public Transform target;
    public float moveSpeed;
    public float rotateSpeed = 0.0025f;

    private Rigidbody2D rb;

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
        rb.velocity = transform.up * moveSpeed;
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
        }
        else if (other.gameObject.CompareTag("Bullet")) {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
