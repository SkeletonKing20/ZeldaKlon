using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity, IDamageable
{
    public delegate void deathCounter();
    public static event deathCounter OnEnemyDeath;
    float killCooldown = 1f;
    public void TakeDamage(Entity damageDealer)
    {
        ReceiveDamage(damageDealer);
    }

    protected override void Die()
    {
        Destroy(gameObject);
        if (OnEnemyDeath != null && Time.deltaTime > killCooldown)
        {
            killCooldown += Time.deltaTime;
            OnEnemyDeath();
        }
    }
}
