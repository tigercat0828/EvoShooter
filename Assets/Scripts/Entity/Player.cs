using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour, IEntity {

    [SerializeField] private int SlotNo = 0;
    public int gHealthPoint = 100;
    public int gAttackPoint = 10;
    public float gMoveSpeed = 15;
    public float gRotateSpeed = 60f;
    public float gFireRate = 2;
    public float gBulletSpeed = 10;
    public int gMagazineSize = 10;
    public float gViewDistance = 10;
    public float gReloadTime = 3;


    public int Fitness = 0;

    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private Transform gunBarrel;
    [SerializeField] private SpriteRenderer reloadingSprite; // Add this line

    private Vector2 _moveInput;
    private Vector3 _mousePosition;

    private float _fireTimer;
    private float _fireInterval;

    [SerializeField] private int _CurrentHP;
    [SerializeField] private int _remainingBullet = 0;
    [SerializeField] private bool _isReloading = false;

    private Rigidbody2D _rigidbody;

    

    private void Awake() {
        LoadGameSettings();
        _rigidbody = GetComponent<Rigidbody2D>();

        _fireInterval = 1 / gFireRate;
        _CurrentHP = gHealthPoint;
        _remainingBullet = gMagazineSize;
        reloadingSprite.gameObject.SetActive(false); // Ensure the loading sprite is initially hidden

        
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
        reloadingSprite.gameObject.SetActive(true); // Show the loading sprite
        yield return new WaitForSeconds(gReloadTime);
        _remainingBullet = gMagazineSize;
        _isReloading = false;
        reloadingSprite.gameObject.SetActive(false); // Hide the loading sprite
    }
    public void TakeDamage(int amount) {
        _CurrentHP -= amount;
        if (_CurrentHP < 0) {   // Die
            Destroy(gameObject);
            Fitness = Globals.GetScore(SlotNo);
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
        gBulletSpeed = GameSettings.options.Agent_BulletSpeed;
        gReloadTime = GameSettings.options.Agent_ReloadTime;
        gFireRate = GameSettings.options.Agent_FireRate;
        gMagazineSize = GameSettings.options.Agent_MagazineSize;
        gViewDistance = GameSettings.options.Agent_ViewDistance;

    }
    public void SetSlot(int slot) {
        SlotNo = slot;
    }
}
