using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    private int currentHP;
    public int initialHP = 3;
    protected SpriteRenderer spriteR;
    protected bool isInvincible;
    private float knockbackStrength;
    private float invincibilityDuration = 1f;
    public LayerMask attackableLayers;

    protected virtual void Awake()
    {
        currentHP = initialHP;
        knockbackStrength = 1f;
        isInvincible = false;

 

        spriteR = GetComponent<SpriteRenderer>();
    }
    public void ReceiveDamage(Entity damageDealer)
    {
        currentHP--;
        Debug.Log($"{currentHP}/{initialHP}");

        Vector3 pushDirection = (transform.position - damageDealer.transform.position).normalized;

        transform.position += pushDirection * knockbackStrength;

        StartCoroutine(InvincibilityCoroutine(invincibilityDuration));

        if (currentHP <= 0)
        {
            Die();
        }
    }
    protected void RestoreHP()
    {
        currentHP = initialHP;
    }
    protected abstract void Die();

    private IEnumerator InvincibilityCoroutine(float duration)
    {
        Color colour = spriteR.color;
        spriteR.color = Color.red;
        isInvincible = true;
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            spriteR.color = Color.Lerp(Color.red, colour, t / duration);
            yield return new WaitForEndOfFrame();
        }

        spriteR.color = colour;
        isInvincible = false;
    }
}
