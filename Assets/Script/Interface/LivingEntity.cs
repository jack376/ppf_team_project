using System;
using UnityEngine;

public class LivingEntity : MonoBehaviour, IDamageable
{
    public float health { get; protected set; } = 100f;
    public bool  dead   { get; protected set; }
    public event Action onDeath;

    protected virtual void OnEnable()
    {
        dead = false;
    }

    public virtual void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0 && !dead)
        {
            Die();
        }
    }

    public virtual void Healing(float newHealth)
    {
        if (dead)
        {
            return;
        }
        health += newHealth;
    }

    public virtual void Die()
    {
        if (onDeath != null)
        {
            onDeath();
        }
        dead = true;
    }

    public virtual void DeleyDie()
    {
        if (onDeath != null)
        {
            onDeath();
        }
    }
}