using UnityEngine;

public class Tank : MonoBehaviour, IEntity {

    [SerializeField] Estate _state;
    public int HealthPoint = 200;
    public int AttackPoint = 20;
    public float MoveSpeed = 5f;
    public float RotateSpeed = 25f;
    public float ViewDistance = 10f;
    [SerializeField] private int _CurrentHP;
    [SerializeField] private float KnockResistance = 0.5f;
    private Transform _target;
    private Rigidbody2D _rigidbody;

    // wander
    private Vector2 _wanderDirection;
    [SerializeField] private float _WanderDirectionChangeInterval = 6f;
    private float _WanderDirectionChangeTimer;

    private void Awake() {
        _rigidbody = GetComponent<Rigidbody2D>();
        _CurrentHP = HealthPoint;
    }
    private void Start() {
        LocateTarget(0);
    }
    private void Update() {
        AwareAgent();
        Quaternion targetRotation = new();
        if (_state == Estate.TargetFound) {
            Vector2 targetDirection = _target.position - transform.position;
            float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg - 90;
            targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, RotateSpeed * Time.deltaTime);
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
    private void FixedUpdate() {
        _rigidbody.AddForce(transform.up * MoveSpeed);
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
        if (_CurrentHP < 0) {
            Destroy(gameObject);
            int score = GameSettings.options.Score_Tank;
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
        _rigidbody.AddForce(direction * strength * KnockResistance, ForceMode2D.Impulse);
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
