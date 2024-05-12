using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public float MoveSpeed { get; set; } = 3f;
    public float RotateSpeed { get; set; } = 0.05f;
    public int HealthPoint { get; set; } = 100;
    public int AttackPoint { get; set; }  = 30;
    
    public int CurrentHP;
    public Transform Target;
    
    protected Rigidbody2D Rigidbody;

    private void Awake() {
        Rigidbody = GetComponent<Rigidbody2D>();
        CurrentHP = HealthPoint;
    }

    protected void RotateTowardTarget() {
        Vector2 targetDirection = Target.position - transform.position;
        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg - 90;
        Quaternion q = Quaternion.Euler(new Vector3(0, 0, angle));
        transform.localRotation = Quaternion.Slerp(transform.localRotation, q, RotateSpeed);
    }
    private void FixedUpdate() {
        Rigidbody.velocity = transform.up * MoveSpeed;
    }
    public void TakeDamage(int amount) {
        CurrentHP -= amount;
        if(CurrentHP < 0) {
            Destroy(gameObject);
        }
    }
    public void Heal(int amount) { 
        CurrentHP += amount;
        if(CurrentHP > HealthPoint) {
            CurrentHP = HealthPoint;
        }
    }
}
