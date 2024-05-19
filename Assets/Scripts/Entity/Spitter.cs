using Unity.VisualScripting;
using UnityEngine;

public class Spitter : MonoBehaviour, IEntity {
    // Here is Zombie
    public int gHealthPoint = 100;
    public int gAttackPoint = 10;
    public float gMoveSpeed = 10;
    public float gRotateSpeed = 60f;
    public float gViewDistance = 5f;
    public float gFireRate = 1;


    [SerializeField] private int _CurrentHP;
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private Transform _FirePoint;
    private Transform _target;
    private Rigidbody2D _rigidbody;
    
    public float _distanceToStop;
    private bool IsTargetFound;
    private float _fireInterval;
    private float _fireTimer;
    // wander
    private Vector2 _wanderDirection;
    [SerializeField] private float _timeToChangeDirection = 2f;
    private float _directionChangeTimer;

    private void Awake() {
        _rigidbody = GetComponent<Rigidbody2D>();
        _distanceToStop = gViewDistance * 0.8f;
        _fireInterval = 1 / gFireRate;
        _CurrentHP = gHealthPoint;
    }
    private void Start() {
        LocateTarget();
    }
    private void Update() {
        _fireTimer -= Time.deltaTime;
        AwareAgent();
        if(IsTargetFound ) { 
            RotateToTarget();
            Shoot();
        }
        else {
            Wander();
        }
        
    }

    private void Shoot() {
        if (_fireTimer < 0f) {
            _fireTimer = _fireInterval;
            Bullet bullet = Instantiate(_bulletPrefab, _FirePoint.position, _FirePoint.rotation);
            bullet.Damage = gAttackPoint;
        }

    }
    private void FixedUpdate() {
        if (IsTargetFound && Vector2.Distance(transform.position, _target.position) > _distanceToStop) {
            _rigidbody.AddForce(transform.up * gMoveSpeed);
        }
        if (!IsTargetFound) {
            _rigidbody.AddForce(_wanderDirection.normalized * gMoveSpeed);
        }
    }
    private void LocateTarget() {
        _target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void RotateToTarget() {
        Vector2 targetDirection = _target.position - transform.position;
        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg - 90;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, gRotateSpeed);
    }

    public void TakeDamage(int amount) {
        _CurrentHP -= amount;
        if (_CurrentHP < 0) {
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
    public void AwareAgent() {
        if(Vector2.Distance(transform.position,_target.position) < gViewDistance) {
            IsTargetFound = true;
        }
        else {
            IsTargetFound = false;
        }
    }
    private void Wander() {
        _directionChangeTimer -= Time.fixedDeltaTime;

        if (_directionChangeTimer <= 0) {
            ChangeWanderDirection();
            _directionChangeTimer = _timeToChangeDirection;
        }

        // Apply the wandering direction as force
        //_rigidbody.AddForce(_wanderDirection.normalized * gMoveSpeed);
        //_rigidbody.AddForce(transform.up * gMoveSpeed);
        // Optionally, you could also rotate towards the wander direction to make it look more natural
        Quaternion toRotation = Quaternion.LookRotation(Vector3.forward, _wanderDirection);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, gRotateSpeed * Time.fixedDeltaTime);
    }

    private void ChangeWanderDirection() {
        // Generate a new direction by choosing a random angle
        float angle = Random.Range(0, 360) * Mathf.Deg2Rad;
        _wanderDirection = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
    }

}
