
using JetBrains.Annotations;
using UnityEngine;


public class Zombie : MonoBehaviour, IEntity {
    public float MoveSpeed    = 3f;
    public float RotateSpeed  = 0.05f;
    public int HealthPoint    = 100;
    public int AttackPoint    = 10;

    [SerializeField] private int CurrentHP;
    private Transform Target;

    private Rigidbody2D _rigidbody;
    private float KnockbackTimer;
    private float KnockbackInterval;
    private bool IsHit;

    private void Awake() {
        _rigidbody = GetComponent<Rigidbody2D>();
        CurrentHP = HealthPoint;
    }

    private void Update() {
        Target = GameObject.FindGameObjectWithTag("Player").transform;
        RotateToTarget();
    }
    private void FixedUpdate() {
        if (!IsHit) {
            _rigidbody.velocity = transform.up * MoveSpeed;
        }
        if(KnockbackTimer > 0) {
            KnockbackTimer -= Time.deltaTime;
        }
        else {
            IsHit = false;
        }
    }
    protected void RotateToTarget() {
        Vector2 targetDirection = Target.position - transform.position;
        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg - 90;
        Quaternion q = Quaternion.Euler(new Vector3(0, 0, angle));
        transform.localRotation = Quaternion.Slerp(transform.localRotation, q, RotateSpeed);
    }
    public void MoveToTarget() {
        Vector3 direction = (Target.position - transform.position).normalized;
        transform.position += MoveSpeed * Time.deltaTime * direction;
    }
    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Player")) {
            IEntity player = other.gameObject.GetComponent<IEntity>();
            player.TakeDamage(AttackPoint);
        }
        if (other.gameObject.CompareTag("Bullet")) {
            IsHit = true;
            KnockbackTimer = KnockbackInterval;
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

    public void KnockBack(Vector2 direction, float strength) {
        _rigidbody.AddForce(direction * strength, ForceMode2D.Impulse);
    }
}
