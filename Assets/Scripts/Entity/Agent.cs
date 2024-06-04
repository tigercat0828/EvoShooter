using System.Collections;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Rendering;


public class Agent : MonoBehaviour, IEntity {
    // 視野內敵人如果距離過近 => 遠離敵人並開火
    // 視野內敵人如果距離過遠 => 追擊敵人並開火?
    // 視野內無敵人 => Wander
    [SerializeField] private int SlotNo = 0;
    [SerializeField] Estate _state;
    public int      Fitness = 0;
    public int      gHealthPoint = 100;
    public int      gAttackPoint = 10;
    public float    gMoveSpeed = 15;
    public float    gRotateSpeed = 60f;
    public float    gFireRate = 2;
    public int      gMagazineSize = 10;
    public float    gReloadTime = 3;
    public float    gBulletSpeed = 10;
    public float    gViewDistance = 10;

    private float _fireTimer;
    private float _fireInterval;
    private float _distanceToStop;

    // wander
    private Vector2 _wanderDirection;
    [SerializeField] private float _wanderDirectionChangeInterval = 6f;
    private float _wanderDirectionChangeTimer;

    [SerializeField] private int _CurrentHP;
    [SerializeField] private int _remainingBullet = 0;
    [SerializeField] private bool _isReloading = false;

    private Rigidbody2D _rigidbody;

    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private Transform gunBarrel;
    [SerializeField] private SpriteRenderer reloadingSprite;

    private Transform _target;

    Transform[] _trackedEnemy;

    private void Awake() {
        LoadGameSettings();
        _rigidbody = GetComponent<Rigidbody2D>();
        _fireInterval = 1 / gFireRate;
        _CurrentHP = gHealthPoint;
        _remainingBullet = gMagazineSize;
        _distanceToStop =gViewDistance * 0.8f;
        reloadingSprite.gameObject.SetActive(false); 
    }
    private void Update() {

        _fireTimer -= Time.deltaTime;
        SenseEnviroment();
        // TODO : finish state transfer
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
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, gRotateSpeed * Time.deltaTime);
    }
    private void FixedUpdate() {
        // 進入射程不會再向target靠近
        if ((_state == Estate.TargetFound && Vector2.Distance(transform.position, _target.position) > _distanceToStop)
            || _state == Estate.Wander) {
            _rigidbody.AddForce(transform.up * gMoveSpeed);
        }
    }

    private void Steer() {
        Vector2 targetDirection = _target.position - transform.position;
        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg - 90f;
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, gRotateSpeed * Time.deltaTime);
    }
    private void Shoot() {
        if (_isReloading) return;
        if (Input.GetMouseButton(0) && _fireTimer < 0f && _remainingBullet > 0) {
            _fireTimer = _fireInterval;
            Bullet bullet = Instantiate(bulletPrefab, gunBarrel.position, gunBarrel.rotation);
            bullet.SetStatus(gAttackPoint, gBulletSpeed);
            _remainingBullet--;
            if (_remainingBullet == 0) {
                StartCoroutine(Reload());
            }
        }
    }
    private IEnumerator Reload() {
        _isReloading = true;
        reloadingSprite.gameObject.SetActive(true);     
        yield return new WaitForSeconds(gReloadTime);
        _remainingBullet = gMagazineSize;
        _isReloading = false;
        reloadingSprite.gameObject.SetActive(false);    
    }
    public void TakeDamage(int amount) {
        _CurrentHP -= amount;
        if (_CurrentHP < 0) {
            Destroy(gameObject);
            HumanGameManager.manager.GameOver();
        }
    }
    public void TakeHeal(int amount) {
        _CurrentHP += amount;
        if (_CurrentHP > gHealthPoint) {
            _CurrentHP = gHealthPoint;
        }
    }

    protected void SenseEnviroment() {
        Collider2D[] colliders =  Physics2D.OverlapCircleAll(transform.position, gViewDistance);

        _target = null;
        float closestDistanceSqr = Mathf.Infinity;
        float dSqrToTarget = 0;
        foreach (Collider2D collider in colliders) {
            Transform t = collider.transform;
            Vector2 directionToTarget = (Vector2)(t.position - transform.position);
            dSqrToTarget = directionToTarget.sqrMagnitude; // Using square magnitude for efficiency

            if (dSqrToTarget < closestDistanceSqr) {
                closestDistanceSqr = dSqrToTarget;
                _target = t;
            }
        }
        // TODO :
        if (_target == null) {
            _state = Estate.Wander;
        }
        else if (_target.CompareTag("EnemyBullet")) {
            _state = Estate.Dodge;  // Dodge enemy bullet
            Debug.Log("Dodging Enemy Bullet");
        }
        else {
            if (Mathf.Sqrt(dSqrToTarget) < _distanceToStop) {
                _state = Estate.Escape;
            }
            else {
                _state = Estate.Pursue;
            }
        }
    }

    public void KnockBack(Vector2 direction, float strength) {
        _rigidbody.AddForce(direction * strength, ForceMode2D.Impulse);
    }

    public void LoadGameSettings() {
        //GameSettings.LoadSettings();
        gHealthPoint = GameSettings.options.Agent_HealthPoint;
        gAttackPoint = GameSettings.options.Agent_AttackPoint;
        gMoveSpeed = GameSettings.options.Agent_MoveSpeed;
        gRotateSpeed = GameSettings.options.Agent_RotateSpeed;
        gFireRate = GameSettings.options.Agent_FireRate;
        gBulletSpeed = GameSettings.options.Agent_BulletSpeed;
        gMagazineSize = GameSettings.options.Agent_MagazineSize;
        gViewDistance = GameSettings.options.Agent_ViewDistance;
        gReloadTime = GameSettings.options.Agent_ReloadTime;

    }


    public void SetSlot(int slot) {
        SlotNo = slot;
    }

    public void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Wall")) {
            TurnBack();
        }
    }
    private void TurnBack() {
        transform.Rotate(0f, 0f, 180f);
        ChangeWanderDirection();
        _rigidbody.velocity = Vector2.zero;
    }
    protected void ChangeWanderDirection() {
        float angle = Random.Range(0, 360) * Mathf.Deg2Rad;
        _wanderDirection = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
    }

    public void SetNumericGene(NumericGene gene) {
        gHealthPoint += gene.HealthPoint * GameSettings.options.Ability_HealthPoint;
        gAttackPoint = gene.AttackPoint * GameSettings.options.Ability_AttackPoint;
        gMoveSpeed = gene.MoveSpeed * GameSettings.options.Ability_MoveSpeed;
        gRotateSpeed = gene.RotateSpeed * GameSettings.options.Ability_RotateSpeed;
        gFireRate = gene.FireRate * GameSettings.options.Ability_FireRate;
        gReloadTime = gene.ReloadTime * GameSettings.options.Ability_ReloadTime;
        gBulletSpeed = gene.BulletSpeed * GameSettings.options.Ability_BulletSpeed;
        gMagazineSize = gene.MagazineSize * GameSettings.options.Ability_MagazineSize;
        gViewDistance = gene.ViewDistance * GameSettings.options.Ability_ViewDistance;
    }
    public NumericGene GetNumericGene() {
        return new NumericGene(gHealthPoint, gAttackPoint, gFireRate, gMagazineSize, gReloadTime, gBulletSpeed, gMoveSpeed, gRotateSpeed, gViewDistance);
    }
}
