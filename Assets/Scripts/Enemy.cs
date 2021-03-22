using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity, IDamageable
{
    public void TakeDamage(Entity damageDealer)
    {
        ReceiveDamage(damageDealer);
    }

    protected override void Die()
    {
        Destroy(gameObject);
    }
}
