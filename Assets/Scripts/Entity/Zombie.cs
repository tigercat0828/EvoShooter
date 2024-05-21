
using JetBrains.Annotations;
using UnityEngine;


public class Zombie : MonoBehaviour, IEntity {
    public int HealthPoint    = 100;
    public int AttackPoint    = 10;
    public float MoveSpeed    = 10f;
    public float RotateSpeed  = 30f;

    [SerializeField] private int _CurrentHP;
    private Transform _target;

    private Rigidbody2D _rigidbody;

    private void Awake() {
        _rigidbody = GetComponent<Rigidbody2D>();
        _CurrentHP = HealthPoint;
    }
    private void Start() {
        LocateTarget(0);
    }
    private void Update() {
        RotateToTarget();
    }
    private void FixedUpdate() {
        _rigidbody.AddForce(transform.up * MoveSpeed);
    }
    protected void RotateToTarget() {
        Vector2 targetDirection = _target.position - transform.position;
        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg - 90;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
        transform.rotation = Quaternion.RotateTowards(transform.localRotation, targetRotation, RotateSpeed);
    }
    
    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Player")) {
            IEntity player = other.gameObject.GetComponent<IEntity>();
            player.TakeDamage(AttackPoint);
            player.KnockBack(transform.up, 8);
        }
    }
    public void LocateTarget(int group) {
        _target = GameObject.FindGameObjectWithTag("Player").transform;
    }
    public void TakeDamage(int amount) {
        _CurrentHP -= amount;
        if(_CurrentHP < 0) {
            Destroy(gameObject);
            HumanPlaySceneManager.manager.IncreaseScore(1);
        }
    }

    public void TakeHeal(int amount) {
        _CurrentHP += amount;
        if (_CurrentHP > HealthPoint) {
            _CurrentHP = HealthPoint;
        }
    }
    public void KnockBack(Vector2 direction, float strength) {
        _rigidbody.AddForce(direction * strength, ForceMode2D.Impulse);
    }
}
