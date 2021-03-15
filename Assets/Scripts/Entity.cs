using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
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
        Debug.Log($"{currentHP}/{initialHP}");
        if (currentHP <= 0)
        {
            Die();
        }
    }

    protected abstract void Die();
}
