using UnityEngine;

public class Player : MonoBehaviour, IEntity {

    public int HealthPoint = 100;
    public int AttackPoint = 10;
    public float FireRate = 2;
    public int MagazineSize = 10;
    public float MoveSpeed = 3;
    public float RotateSpeed = 3f;
    public float ViewDistance = 10;

    public int CurrentHP;

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform gunBarrel;


    private Vector2 _moveInput;
    private Vector3 _mousePosition;
    private float fireTimer;
    private float fireInterval;
    private float damageTimer;
    private readonly float damageInterval = 0.5f;
    private Rigidbody2D _rigidbody;

    private void Awake() {
        _rigidbody = GetComponent<Rigidbody2D>();
        fireInterval = 1 / FireRate;
        CurrentHP = HealthPoint;
    }
    private void Update() {
        fireTimer -= Time.deltaTime;
        damageTimer -= Time.deltaTime;
        Move();
        Steer();
        Shoot();
    }
    private void FixedUpdate() {
        _rigidbody.AddForce(MoveSpeed * _moveInput);
    }
    private void Move() {

        _moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
    }
    private void Steer() {
        _mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _mousePosition.z = 0;
        float angle = Mathf.Atan2(_mousePosition.y - transform.position.y, _mousePosition.x - transform.position.x) * Mathf.Rad2Deg-90f ;
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, RotateSpeed * Time.deltaTime);
    }
    private void Shoot() {

        if (Input.GetMouseButton(0) && fireTimer < 0f) {
            fireTimer = fireInterval;
            var bullet = Instantiate(bulletPrefab, gunBarrel.position, gunBarrel.rotation);
            bullet.GetComponent<Bullet>().Damage = AttackPoint;
        }
    }

    public void TakeDamage(int amount) {
        if (damageTimer < 0) {
            damageTimer = damageInterval;
            CurrentHP -= amount;
            if (CurrentHP < 0) {
                Destroy(gameObject);
                HumanPlaySceneManager.manager.GameOver();
            }
        }
    }
    public void TakeHeal(int amount) {
        CurrentHP += amount;
        if (CurrentHP > HealthPoint) {
            CurrentHP = HealthPoint;
        }
    }


    public void KnockBack(Vector2 direction, float strength) {
        _rigidbody.AddForce(direction * strength, ForceMode2D.Impulse);
    }
}
