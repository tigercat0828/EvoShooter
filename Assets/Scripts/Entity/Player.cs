using Unity.VisualScripting;
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
    private float mx;
    private float my;
    private Vector2 mousePos;
    private float fireTimer;
    private float fireInterval;
    private float damageTimer;
    private readonly float damageInterval = 0.5f;

    private void Awake() {
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

    private void Move() {
        mx = Input.GetAxisRaw("Horizontal");
        my = Input.GetAxisRaw("Vertical");
        Vector2 direction = MoveSpeed * Time.deltaTime * new Vector2(mx, my).normalized;
        transform.position += (Vector3)direction;
    }
    private void Steer() {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float angle = Mathf.Atan2(mousePos.y - transform.position.y, mousePos.x - transform.position.x) * Mathf.Rad2Deg - 90f;
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, RotateSpeed * Time.deltaTime);
    }
    private void Shoot() {

        if (Input.GetMouseButton(0) && fireTimer < 0f) {
            fireTimer = fireInterval;
            var bullet = Instantiate(bulletPrefab, gunBarrel.position, gunBarrel.rotation);
            bullet.GetComponent<Bullet>().Damage = AttackPoint;
        }
    }
    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("EnemyBullet")) {
            TakeDamage(10);
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
    public void SetGene(Gene gene) {
        HealthPoint = gene.HealthPoint;
        AttackPoint = gene.AttackPoint;
        FireRate = gene.FireRate;
        MagazineSize = gene.MagazineSize;
        MoveSpeed = gene.MoveSpeed;
        RotateSpeed = gene.RotateSpeed;
        ViewDistance = gene.ViewDistance;
    }
    public Gene GetGene() {
        return new Gene(HealthPoint, AttackPoint, FireRate, MagazineSize, MoveSpeed, RotateSpeed, ViewDistance);
    }
}
