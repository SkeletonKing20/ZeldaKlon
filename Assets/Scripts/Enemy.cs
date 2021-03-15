using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity, IDamageable
{
    protected override void Die()
    {
        Destroy(gameObject);
    }
}
