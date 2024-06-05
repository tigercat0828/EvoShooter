using UnityEngine;



public class Zombie : MonoBehaviour, IEntity {

    [SerializeField] private int SlotNo = 0;
    int IEntity.SlotNo => SlotNo;
    public int  HealthPoint;
    public int  AttackPoint;
    public float MoveSpeed;
    public float RotateSpeed;

    [SerializeField] private int _CurrentHP;
    [SerializeField] private Transform _target;
    private Rigidbody2D _rigidbody;

    public void SetSlotNo(int slot) {
        SlotNo = slot;
    }
    private void Awake() {
        LoadGameSettings();
        _rigidbody = GetComponent<Rigidbody2D>();
        _CurrentHP = HealthPoint;
        HealthPoint = GameSettings.options.Zombie_HealthPoint;
        AttackPoint = GameSettings.options.Zombie_AttackPoint;
        MoveSpeed = GameSettings.options.Zombie_MoveSpeed;
        RotateSpeed = GameSettings.options.Zombie_RotateSpeed;
    }
    private void Start() {
        LocateTarget(0);
    }
    private void Update() {
        if (Globals.intance.ArenaClosed[SlotNo]) {
            Destroy(gameObject);
            return;
        }

        Vector2 targetDirection = _target.position - transform.position;
        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg - 90;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
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

    
    private void TurnBack() {
        // Reverse direction by rotating 180 degrees
        transform.Rotate(0f, 0f, 180f);
        // Reset any forces applied during charge
        _rigidbody.velocity = Vector2.zero;
    }
    public void LocateTarget(int group) {
        //_target = GameObject.FindGameObjectWithTag("Player").transform;
        GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject obj in objectsWithTag) {
            IEntity agents = obj.GetComponent<IEntity>();
            if (agents != null && agents.SlotNo == SlotNo) {
                _target = obj.transform;
                break;
            }
        }
    }
    public void TakeDamage(int amount) {
        _CurrentHP -= amount;
        if (_CurrentHP < 0) {
            Destroy(gameObject);
            Globals.intance.AddScore(SlotNo, GameSettings.options.Score_Zombie);
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

    public void LoadGameSettings() {
        GameSettings.LoadSettings();
        HealthPoint = GameSettings.options.Zombie_HealthPoint;
        AttackPoint = GameSettings.options.Zombie_AttackPoint;
        MoveSpeed = GameSettings.options.Zombie_MoveSpeed;
        RotateSpeed = GameSettings.options.Zombie_RotateSpeed;
    }
    public void SetSlot(int slot) {
        SlotNo = slot;
    }
}
