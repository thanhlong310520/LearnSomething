using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float health = 100;
    public void TakeDamage(float damage)
    {
        health -= damage;
    }
    public bool IsDead => health <= 0;
}
