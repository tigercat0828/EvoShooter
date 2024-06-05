using UnityEngine;

public enum BulletType {
    Agent, Enemy
}
public class Bullet : MonoBehaviour {

    private float Speed = 10f;
    private BulletType Type = BulletType.Agent;
    private int Damage = 10;

    [Range(1, 10)]
    [SerializeField] private float lifeTime = 3f;
    public void SetStatus(int damage, float speed, BulletType type = BulletType.Agent) {
        Speed = speed;
        Damage = damage;
        Type = type;
    }

    private void Start() {
        Destroy(gameObject, lifeTime);
    }

    private void FixedUpdate() {
        transform.position += Speed * Time.fixedDeltaTime * transform.up;
    }
    public void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Wall")) {
            Destroy(gameObject);
            return;
        }
        if (Type == BulletType.Enemy && collision.gameObject.CompareTag("Enemy")) {
            Destroy(gameObject);
            return;
        }
        if (collision.gameObject.TryGetComponent<IEntity>(out var entity)) {
            entity.TakeDamage(Damage);
            entity.KnockBack(transform.up, 8);
            Destroy(gameObject);
        }
    }
}
