using System.Collections;
using UnityEngine;
using UnityEngine.Assertions.Must;


public class Agent : MonoBehaviour, IEntity {

    public int      gHealthPoint = 100;
    public int      gAttackPoint = 10;
    public float    gMoveSpeed = 15;
    public float    gRotateSpeed = 60f;
    public float    gFireRate = 2;
    public float    gBulletSpeed = 10;
    public int      gMagazineSize = 10;
    public float    gViewDistance = 10;
    public float    gReloadingTime = 3;

    private Vector2 _moveInput;
    private Vector3 _mousePosition;

    private float _fireTimer;
    private float _fireInterval;

    [SerializeField] private int _CurrentHP;
    [SerializeField] private int _remainingBullet = 0;
    [SerializeField] private bool _isReloading = false;

    private Rigidbody2D _rigidbody;

    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private Transform gunBarrel;
    [SerializeField] private SpriteRenderer reloadingSprite;



    private void Awake() {
        LoadGameSettings();
        _rigidbody = GetComponent<Rigidbody2D>();

        _fireInterval = 1 / gFireRate;
        _CurrentHP = gHealthPoint;
        _remainingBullet = gMagazineSize;
        reloadingSprite.gameObject.SetActive(false); 
    }
    private void Update() {

        _fireTimer -= Time.deltaTime;
        Move();
        Steer();
        Shoot();
    }
    private void FixedUpdate() {
        _rigidbody.AddForce(gMoveSpeed * _moveInput);
    }
    private void Move() {

        _moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
    }
    private void Steer() {
        _mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _mousePosition.z = 0;
        float angle = Mathf.Atan2(_mousePosition.y - transform.position.y, _mousePosition.x - transform.position.x) * Mathf.Rad2Deg - 90f;
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
        yield return new WaitForSeconds(gReloadingTime);
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


    public void KnockBack(Vector2 direction, float strength) {
        _rigidbody.AddForce(direction * strength, ForceMode2D.Impulse);
    }

    public void LoadGameSettings() {
        GameSettings.LoadSettings();
        gHealthPoint = GameSettings.options.Agent_HealthPoint;
        gAttackPoint = GameSettings.options.Agent_AttackPoint;
        gMoveSpeed = GameSettings.options.Agent_MoveSpeed;
        gRotateSpeed = GameSettings.options.Agent_RotateSpeed;
        gFireRate = GameSettings.options.Agent_FireRate;
        gBulletSpeed = GameSettings.options.Agent_BulletSpeed;
        gMagazineSize = GameSettings.options.Agent_MagazineSize;
        gViewDistance = GameSettings.options.Agent_ViewDistance;
        gReloadingTime = GameSettings.options.Agent_ReloadTime;

    }

    public void SetNumericGene(NumericGene gene) {
        gHealthPoint += gene.HealthPoint * GameSettings.options.Ability_HealthPoint;
        gAttackPoint = gene.AttackPoint * GameSettings.options.Ability_AttackPoint;
        gMoveSpeed = gene.MoveSpeed * GameSettings.options.Ability_MoveSpeed;
        gRotateSpeed = gene.RotateSpeed * GameSettings.options.Ability_RotateSpeed;
        gFireRate = gene.FireRate * GameSettings.options.Ability_FireRate;
        gReloadingTime = gene.ReloadTime * GameSettings.options.Ability_ReloadTime;
        gBulletSpeed = gene.BulletSpeed * GameSettings.options.Ability_BulletSpeed;
        gMagazineSize = gene.MagazineSize * GameSettings.options.Ability_MagazineSize;
        gViewDistance = gene.ViewDistance * GameSettings.options.Ability_ViewDistance;
    }
    public NumericGene GetNumericGene() {
        return new NumericGene(gHealthPoint, gAttackPoint, gFireRate, gMagazineSize, gReloadingTime, gMoveSpeed, gRotateSpeed, gViewDistance);
    }
    public void RandomNumericGene() {

        int[] stats = new int[9];
        int total = 0;

        // Generate random values and calculate their total
        for (int i = 0; i < stats.Length; i++) {
            stats[i] = Random.Range(1, 101);
            total += stats[i];
        }
        // Normalize the values
        double scaleFactor = 100.0 / total;
        for (int i = 0; i < stats.Length; i++) {
            stats[i] = (int)(stats[i] * scaleFactor);
        }
        total = 0;
        for (int i = 0; i < stats.Length; i++) {
            total += stats[i];
        }
        // If the sum is not exactly 100 due to rounding, adjust the last element
        if (total != 100) {
            stats[^1] += 100 - total;
        }

        gHealthPoint = stats[0];
        gAttackPoint = stats[1];
        gMoveSpeed = stats[2];
        gRotateSpeed = stats[3];
        gFireRate = stats[4];
        gReloadingTime = stats[5];
        gBulletSpeed = stats[6];
        gMagazineSize = stats[7];
        gViewDistance = stats[8];
    }

}
