using UnityEngine;


public class Bullet : MonoBehaviour {
    public enum BulletType {
        Agent , Enemy
    }

    public float _speed = 10f;
    public BulletType _type ;
    [Range(1, 10)]
    [SerializeField] private float lifeTime = 3f;

    public int Damage { get; set; } = 10;

    private void Start() {
        Destroy(gameObject, lifeTime);
    }
    private void Update() {
        transform.position += _speed * Time.deltaTime * transform.up;
    }
    public void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Wall")) {
            Destroy(gameObject);
            return;
        }
        if(collision.gameObject.TryGetComponent<IEntity>(out var entity)) {
            entity.TakeDamage(Damage);
            entity.KnockBack(transform.up, 8);
            Destroy(gameObject);
        }
        
    }
}
