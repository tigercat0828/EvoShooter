
using System.Collections;
using UnityEngine;

public class Charger : MonoBehaviour, IEntity {
    // Basic status
    [SerializeField] Estate _state;
    public int HealthPoint = 80;
    public int AttackPoint = 10;
    public float MoveSpeed = 10;
    public float RotateSpeed = 50f;
    //------------------------------
    public float ViewDistance = 30f;
    public float FireRate = 0.5f;

    [SerializeField] protected int _CurrentHP;

    private Transform _target;
    private Rigidbody2D _rigidbody;

    private float _distanceToStop;
    [SerializeField] public float _chargeStrength = 10;
    private float _fireInterval;
    private float _fireTimer;

    // wander
    private Vector2 _wanderDirection;
    [SerializeField] private float _WanderDirectionChangeInterval = 6f;
    private float _WanderDirectionChangeTimer;

    private void Awake() {
        _rigidbody = GetComponent<Rigidbody2D>();
        _distanceToStop = ViewDistance * 0.8f;
        _fireInterval = 1 / FireRate;
        _CurrentHP = HealthPoint;
    }
    private void Start() {

        LocateTarget(0);
        _state = Estate.Wander;
    }
    private void Update() {
        _fireTimer -= Time.deltaTime;
        AwareAgent();

        Quaternion targetRotation = new();
        if (_state == Estate.TargetFound) {
            Charge();
            // Rotate toward target
            Vector2 targetDirection = _target.position - transform.position;
            float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg - 90;
            targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
        if (_state == Estate.Wander) {
            _WanderDirectionChangeTimer -= Time.deltaTime;
            if (_WanderDirectionChangeTimer <= 0) {
                _WanderDirectionChangeTimer = _WanderDirectionChangeInterval;
                ChangeWanderDirection();
            }
            targetRotation = Quaternion.LookRotation(Vector3.forward, _wanderDirection);
        }
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, RotateSpeed * Time.deltaTime);
    }

    private void Charge() {
        
        float angle = Vector2.Angle(transform.up, _target.position - transform.position);

        if (_fireTimer < 0f && angle < 10) {
            _fireTimer = _fireInterval;
            _rigidbody.AddForce(transform.up*_chargeStrength, ForceMode2D.Impulse);
        }
    }
    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Player")) {
            IEntity player = other.gameObject.GetComponent<IEntity>();
            player.TakeDamage(AttackPoint);
            player.KnockBack(transform.up, 16);
        }
    }
    private void FixedUpdate() {
        // 進入射程不會再向Agent靠近
        if ((_state == Estate.TargetFound && Vector2.Distance(transform.position, _target.position) > _distanceToStop)
            || _state == Estate.Wander) {
            _rigidbody.AddForce(transform.up * MoveSpeed);
        }
    }
    protected void LocateTarget(int group) {
        _target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void TakeDamage(int amount) {
        _CurrentHP -= amount;
        if (_CurrentHP < 0) {
            Destroy(gameObject);
            int score = GameSettings.options.Score_Spitter;
            HumanPlaySceneManager.manager.IncreaseScore(score);
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
    protected void AwareAgent() {
        if (Vector2.Distance(transform.position, _target.position) < ViewDistance) {
            _state = Estate.TargetFound;
        }
        else {
            _state = Estate.Wander;
        }
    }
    protected void ChangeWanderDirection() {
        float angle = Random.Range(0, 360) * Mathf.Deg2Rad;
        _wanderDirection = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
    }
}
