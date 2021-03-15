using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    int currentHP;
    public int initialHP = 3;

    private void Awake()
    {
        currentHP = initialHP;
    }
    public void TakeDamage()
    {
        currentHP--;

        if (currentHP <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
