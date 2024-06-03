using UnityEngine;

public class Tank : MonoBehaviour, IEntity {

    [SerializeField] Estate _state;
    public int HealthPoint;
    public int AttackPoint;
    public float MoveSpeed;
    public float RotateSpeed;
    public float ViewDistance;

    [SerializeField] private int _CurrentHP;
    [SerializeField] private float _KnockResistance = 0.2f;
    private Transform _target;
    private Rigidbody2D _rigidbody;

    // wander
    private Vector2 _wanderDirection;
    [SerializeField] private float _WanderDirectionChangeInterval = 6f;
    private float _WanderDirectionChangeTimer;

    private void Awake() {
        LoadGameSettings();
        _rigidbody = GetComponent<Rigidbody2D>();
        _CurrentHP = HealthPoint;
        HealthPoint = GameSettings.options.Tank_HealthPoint;
        AttackPoint = GameSettings.options.Tank_AttackPoint;
        MoveSpeed = GameSettings.options.Tank_MoveSpeed;
        RotateSpeed = GameSettings.options.Tank_RotateSpeed;
        ViewDistance = GameSettings.options.Tank_ViewDistance;
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
        else if (other.gameObject.CompareTag("Wall")) {
            TurnBack();
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
            HumanGameManager.manager.IncreaseScore(score);
        }
    }

    public void TakeHeal(int amount) {
        _CurrentHP += amount;
        if (_CurrentHP > HealthPoint) {
            _CurrentHP = HealthPoint;
        }
    }
    public void KnockBack(Vector2 direction, float strength) {
        _rigidbody.AddForce(_KnockResistance * strength * direction, ForceMode2D.Impulse);
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

    private void TurnBack() {
        // Reverse direction by rotating 180 degrees
        transform.Rotate(0f, 0f, 180f);

        // Optionally change wander direction to avoid getting stuck
        ChangeWanderDirection();

        // Reset any forces applied during charge
        _rigidbody.velocity = Vector2.zero;
    }

    public void LoadGameSettings() {
        HealthPoint = GameSettings.options.Tank_HealthPoint;
        AttackPoint = GameSettings.options.Tank_AttackPoint;
        MoveSpeed = GameSettings.options.Tank_MoveSpeed;
        RotateSpeed = GameSettings.options.Tank_RotateSpeed; 
        ViewDistance = GameSettings.options.Tank_ViewDistance;
    }
}
