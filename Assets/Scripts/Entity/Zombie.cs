
using UnityEngine;


public class Zombie : MonoBehaviour, IEntity {
    public float MoveSpeed { get; set; } = 3f;
    public float RotateSpeed { get; set; } = 0.05f;
    public int HealthPoint { get; set; } = 100;
    public int AttackPoint { get; set; } = 10;

    public int CurrentHP;
    public Transform Target;

    protected Rigidbody2D Rigidbody;

    private void Awake() {
        Rigidbody = GetComponent<Rigidbody2D>();
        CurrentHP = HealthPoint;
    }

    private void Update() {
        Target = GameObject.FindGameObjectWithTag("Player").transform;
        RotateTowardTarget();
    }
    private void FixedUpdate() {
        Rigidbody.velocity = transform.up * MoveSpeed;
    }
    protected void RotateTowardTarget() {
        Vector2 targetDirection = Target.position - transform.position;
        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg - 90;
        Quaternion q = Quaternion.Euler(new Vector3(0, 0, angle));
        transform.localRotation = Quaternion.Slerp(transform.localRotation, q, RotateSpeed);
    }
    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Player")) {
            IEntity player = other.gameObject.GetComponent<IEntity>();
            player.TakeDamage(AttackPoint);
        }
    }

    public void TakeDamage(int amount) {
        CurrentHP -= amount;
        if(CurrentHP < 0) {
            Destroy(gameObject);
            HumanPlaySceneManager.manager.IncreaseScore(1);
        }
    }

    public void TakeHeal(int amount) {
        CurrentHP += amount;
        if (CurrentHP > HealthPoint) {
            CurrentHP = HealthPoint;
        }
    }
}
