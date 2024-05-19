using UnityEngine;

public class Bullet : MonoBehaviour {
    [Range(1, 10)]
    [SerializeField] private float speed = 10f;

    [Range(1, 10)]
    [SerializeField] private float lifeTime = 3f;

    public int Damage { get; set; } = 10;

    private void Start() {
        Destroy(gameObject, lifeTime);
    }
    private void Update() {
        transform.position += transform.up * speed * Time.deltaTime;
    }
    public void OnCollisionEnter2D(Collision2D collision) {

        if(collision.gameObject.TryGetComponent<IEntity>(out var entity)) {
            Vector2 Direction = (collision.transform.position - transform.position).normalized;
            entity.TakeDamage(Damage);
            entity.KnockBack(Direction.normalized, 50);
        }
        Destroy(gameObject);
    }
}
