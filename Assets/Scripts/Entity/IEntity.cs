using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEntity {
    int HealtPoint { get; set; }
    float MoveSpeed { get; set; }
    float RotateSpeed { get; set; }
    void Attack(int damage);
    void TakeDamage(int amount);
}
