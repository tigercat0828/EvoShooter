using System.Collections;
using UnityEngine;

public class Agent : MonoBehaviour, IEntity {
    // 視野內敵人如果距離過近 => 遠離敵人並開火
    // 視野內敵人如果距離過遠 => 追擊敵人並開火?
    // 視野內無敵人 => Wander

    [SerializeField] private int SlotNo = 0;
    int IEntity.SlotNo => SlotNo;
    [SerializeField] Estate _state;

    [SerializeField] public Gene Gene;

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
    
    public float    KeepAwayFactor = 0.8f;
    public float    DodgeFactor = 0.25f;
    
    private float _fireTimer;
    private float _fireInterval;
    private float _distanceToKeepAway;
    private float _distanceToDodge;

    
    public bool IsInEvoScene = true;
    [SerializeField] private float _dodgeStrength = 1;


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

    [SerializeField] private Transform _target;

    Transform[] _trackedEnemy;

    private void Awake() {
        _rigidbody = GetComponent<Rigidbody2D>();
       
    }
    private void Start() {
        _fireInterval = 1 / gFireRate;
        _CurrentHP = gHealthPoint;
        _remainingBullet = gMagazineSize;
        _distanceToKeepAway = gViewDistance * KeepAwayFactor;
        _distanceToDodge = gViewDistance * DodgeFactor;
        reloadingSprite.gameObject.SetActive(false);
    }

    private void FixedUpdate() {

        _fireTimer -= Time.fixedDeltaTime;
        FindTarget();
        DodgeBullet();
        Quaternion targetRotation = new();
        if (_state == Estate.Pursue || _state == Estate.Escape) {
            Shoot();
            // Rotate toward target
            Vector2 targetDirection = _target.position - transform.position;
            float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg - 90;
            targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
        if (_state == Estate.Wander) {
            if (_remainingBullet != gMagazineSize) {
                StartCoroutine(Reload());
            }
            _wanderDirectionChangeTimer -= Time.fixedDeltaTime;
            if (_wanderDirectionChangeTimer <= 0) {
                _wanderDirectionChangeTimer = _wanderDirectionChangeInterval;
                ChangeWanderDirection();
            }
            targetRotation = Quaternion.LookRotation(Vector3.forward, _wanderDirection);
        }
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, gRotateSpeed * Time.fixedDeltaTime);

        // 進入射程不會再向target靠近
        if (_state == Estate.Pursue || _state == Estate.Wander) {
            _rigidbody.AddForce(transform.up * gMoveSpeed);
        }
        else if (_state == Estate.Escape) {
            _rigidbody.AddForce(-transform.up * gMoveSpeed);
        }
    }

    private void Shoot() {
        if (_isReloading) return;
        if(_target != null) {
            // 槍口與敵人的夾角
            float angle = Vector2.Angle(transform.up, _target.position - transform.position);
            if (angle < 3f && _fireTimer < 0f && _remainingBullet > 0) {
                _fireTimer = _fireInterval;
                Bullet bullet = Instantiate(bulletPrefab, gunBarrel.position, gunBarrel.rotation);
                bullet.SetStatus(gAttackPoint, gBulletSpeed);
                _remainingBullet--;
                if (_remainingBullet == 0) {
                    StartCoroutine(Reload());
                }
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
            if (!IsInEvoScene) {
                GameLevelManager.manager.GameOver();    // TODO : fix here
            }
            Fitness = Globals.instance.GetScore(SlotNo);
            Globals.instance.ArenaClosed[SlotNo] = true;
        }
    }
    public void TakeHeal(int amount) {
        _CurrentHP += amount;
        if (_CurrentHP > gHealthPoint) {
            _CurrentHP = gHealthPoint;
        }
    }
    private void DodgeBullet() {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _distanceToDodge);
        foreach (Collider2D collider in colliders) {
            Transform t = collider.transform;
            if (t.CompareTag("EnemyBullet")) {
                Vector3 dodgeDirection = (Vector3)Vector2.Perpendicular(t.up).normalized;
                Vector3 directionToSpit = t.transform.position - transform.position;
                if(Vector3.Dot(directionToSpit, dodgeDirection) > 0 ) {
                    dodgeDirection = -dodgeDirection;
                }
                _rigidbody.AddForce(dodgeDirection * _dodgeStrength, ForceMode2D.Impulse);
                break;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Wall") && _state == Estate.Wander) {
            TurnBack();
        }
    }
    private void TurnBack() {
        // Reverse direction by rotating 180 degrees
        transform.Rotate(0f, 0f, 180f);

        // Optionally change wander direction to avoid getting stuck
        ChangeWanderDirection();

        // Reset any forces applied during charge
        _rigidbody.velocity = Vector2.zero;
    }
    private void FindTarget() {
        Collider2D[] colliders =  Physics2D.OverlapCircleAll(transform.position, gViewDistance);

        _target = null;
        float closestDistanceSqr = Mathf.Infinity;
        float dSqrToTarget = 0;
        foreach (Collider2D collider in colliders) {
            Transform t = collider.transform;
            if (t.CompareTag("EnemyBullet") || t.CompareTag("Player") ||t.CompareTag("Wall" ) || t.CompareTag("Bullet")) continue;
            
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
        else { 
            if (Mathf.Sqrt(dSqrToTarget) < _distanceToKeepAway) {   // 離敵人太近=>遠離
                _state = Estate.Escape;
            }
            else {  // 離敵人太遠 追擊
                _state = Estate.Pursue;                                 
            }
        }
    }

    public void KnockBack(Vector2 direction, float strength) {
        _rigidbody.AddForce(direction * strength, ForceMode2D.Impulse);
    }

    public void LoadGameSettings() {
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
   
    protected void ChangeWanderDirection() {
        float angle = Random.Range(0, 360) * Mathf.Deg2Rad;
        _wanderDirection = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
    }

    public void SetGene(Gene gene) {
        Gene = gene;
        gHealthPoint += gene.HealthPoint * GameSettings.options.Ability_HealthPoint;
        gAttackPoint += gene.AttackPoint * GameSettings.options.Ability_AttackPoint;
        gMoveSpeed += gene.MoveSpeed * GameSettings.options.Ability_MoveSpeed;
        gRotateSpeed += gene.RotateSpeed * GameSettings.options.Ability_RotateSpeed;
        gFireRate += gene.FireRate * GameSettings.options.Ability_FireRate;
        gReloadTime -= gene.ReloadTime * GameSettings.options.Ability_ReloadTime;
        gBulletSpeed += gene.BulletSpeed * GameSettings.options.Ability_BulletSpeed;
        gMagazineSize += gene.MagazineSize * GameSettings.options.Ability_MagazineSize;
        gViewDistance += gene.ViewDistance * GameSettings.options.Ability_ViewDistance;
    }
    public Gene GetNumericGene() {
        return Gene;
    }
}
