using UnityEngine;

public class Spitter : MonoBehaviour, IEntity {

    [SerializeField] private int SlotNo = 0;
    int IEntity.SlotNo => SlotNo;
    // Basic status
    [SerializeField] Estate _state;
    public int   HealthPoint;
    public int   AttackPoint;
    public float MoveSpeed;
    public float RotateSpeed;
    public float ViewDistance;
    public float FireRate;

    [SerializeField] protected int _CurrentHP;
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private Transform _FirePoint;

    private Transform _target;
    private Rigidbody2D _rigidbody;

    private float _distanceToStop;
    private float _fireInterval;
    private float _fireTimer;
    // wander
    private Vector2 _wanderDirection;
    [SerializeField] private float _wanderDirectionChangeInterval = 6f;
    private float _wanderDirectionChangeTimer;

    private void Awake() {
        LoadGameSettings();
        _rigidbody = GetComponent<Rigidbody2D>();
        _CurrentHP = HealthPoint;
        _distanceToStop = ViewDistance * 0.8f;
        _fireInterval = 1 / FireRate;
    }
    private void Start() {

        LocateTarget(0);
        _state = Estate.Wander;
    }

    private void Shoot() {
        float angle = Vector2.Angle(transform.up, _target.position - transform.position);

        if (_fireTimer < 0f && angle < 30) {
            _fireTimer = _fireInterval;
            Bullet bullet = Instantiate(_bulletPrefab, _FirePoint.position, _FirePoint.rotation);
            bullet.SetStatus(AttackPoint, 10, BulletType.Enemy);
        }
    }
    private void FixedUpdate() {
        if (!AwareAgent()) return;
        _fireTimer -= Time.fixedDeltaTime;

        Quaternion targetRotation = new();
        if (_state == Estate.TargetFound) {
            Shoot();
            // Rotate toward target
            Vector2 targetDirection = _target.position - transform.position;
            float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg - 90;
            targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
        if (_state == Estate.Wander) {
            _wanderDirectionChangeTimer -= Time.deltaTime;
            if (_wanderDirectionChangeTimer <= 0) {
                _wanderDirectionChangeTimer = _wanderDirectionChangeInterval;
                ChangeWanderDirection();
            }
            targetRotation = Quaternion.LookRotation(Vector3.forward, _wanderDirection);
        }
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, RotateSpeed * Time.fixedDeltaTime);
        // 進入射程不會再向Agent靠近
        if ((_state == Estate.TargetFound && Vector2.Distance(transform.position, _target.position) > _distanceToStop)
            || _state == Estate.Wander) {
            _rigidbody.AddForce(transform.up * MoveSpeed);
        }
    }
    protected void LocateTarget(int slot) {
        // _target = GameObject.FindGameObjectWithTag("Player").transform;
        GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject obj in objectsWithTag) {
            IEntity agents = obj.GetComponent<IEntity>();
            if (agents != null && agents.SlotNo == SlotNo) {
                _target = obj.transform;
                break;
            }
        }

    }
    public void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Wall")) {
            TurnBack();
        }
    }
    public void TakeDamage(int amount) {
        _CurrentHP -= amount;
        if (_CurrentHP < 0) {
            Destroy(gameObject);
            Globals.instance.AddScore(SlotNo, GameSettings.options.Score_Spitter);
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
    protected bool AwareAgent() {
        if (Globals.instance.ArenaClosed[SlotNo]) {
            Destroy(gameObject);
            return false;
        }
        if (Vector2.Distance(transform.position, _target.position) < ViewDistance) {
            _state = Estate.TargetFound;
        }
        else {
            _state = Estate.Wander;
        }
        return true;
    }
    protected void ChangeWanderDirection() {
        float angle = Random.Range(0, 360) * Mathf.Deg2Rad;
        _wanderDirection = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
    }
    private void TurnBack() {
        transform.Rotate(0f, 0f, 180f);
        ChangeWanderDirection();
        _rigidbody.velocity = Vector2.zero;
    }

    public void LoadGameSettings() {
        GameSettings.LoadSettings();
        HealthPoint = GameSettings.options.Spitter_HealthPoint;
        AttackPoint = GameSettings.options.Spitter_AttackPoint ;
        MoveSpeed = GameSettings.options.Spitter_MoveSpeed;
        RotateSpeed = GameSettings.options.Spitter_RotateSpeed;
        ViewDistance = GameSettings.options.Spitter_ViewDistance;
        FireRate = GameSettings.options.Spitter_FireRate;
    }

    public void SetSlot(int slot) {
        SlotNo = slot;
    }
}
