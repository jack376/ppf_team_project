using System;
using UnityEngine;

public class LivingEntity : MonoBehaviour, IDamageable
{
    // LivingEntity 상속 받은 모든 객체의 생명력, 디폴트 값 100
    public float health { get; protected set; } = 100f;
    
    // 죽었는지 살았는지 유무
    public bool  dead   { get; protected set; }

    // 대상이 죽었을 때 호출되는 이벤트 (델리게이트)
    public event Action onDeath; 

    protected virtual void OnEnable()
    {
        dead = false;
    }

    // damage(피해량) 만큼 health(체력) 감소, 죽지 않은 상태에서 health가 0이 되면 Die() 메서드 호출
    public virtual void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        health -= damage;
        if (health <= 0 && !dead)
        {
            Die();
        }
    }

    // 죽지 않은 상태일 경우 전달된 인자만큼 체력 회복
    public virtual void Healing(float newHealth)
    {
        if (dead)
        {
            return;
        }
        health += newHealth;
    }

    // Die 호출 시 onDeath 이벤트 호출, dead 불리언 값 true로 변경
    public virtual void Die()
    {
        if (onDeath != null)
        {
            onDeath();
        }
        dead = true;
    }
}