using UnityEngine;

public interface IEntity {
    public void TakeDamage(int amount);
    public void TakeHeal(int amount);
    public void KnockBack(Vector2 direction, float strength);
}
