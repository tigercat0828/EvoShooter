
using JetBrains.Annotations;
using UnityEngine;


public class Zombie : MonoBehaviour, IEntity {
    public int gHealthPoint    = 100;
    public int gAttackPoint    = 10;
    public float gMoveSpeed    = 30f;
    public float gRotateSpeed  = 30f;

    [SerializeField] private int _CurrentHP;
    private Transform _target;

    private Rigidbody2D _rigidbody;

    private void Awake() {
        _rigidbody = GetComponent<Rigidbody2D>();
        _CurrentHP = gHealthPoint;
    }
    private void Start() {
        LocateTarget();
    }
    private void Update() {
        RotateToTarget();
    }
    private void FixedUpdate() {
        _rigidbody.AddForce(transform.up * gMoveSpeed);
    }
    protected void RotateToTarget() {
        Vector2 targetDirection = _target.position - transform.position;
        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg - 90;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
        transform.rotation = Quaternion.RotateTowards(transform.localRotation, targetRotation, gRotateSpeed);
    }
    
    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Player")) {
            IEntity player = other.gameObject.GetComponent<IEntity>();
            player.TakeDamage(gAttackPoint);
            player.KnockBack(transform.up, 8);
        }
    }
    public void LocateTarget() {
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
        if (_CurrentHP > gHealthPoint) {
            _CurrentHP = gHealthPoint;
        }
    }
    public void KnockBack(Vector2 direction, float strength) {
        _rigidbody.AddForce(direction * strength, ForceMode2D.Impulse);
    }
}
