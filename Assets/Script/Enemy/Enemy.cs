using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : LivingEntity
{
    [Header("몬스터 데이터"), Space(5f)]
    public EnemyData enemyData;

    [Header("추적할 대상 레이어"), Space(5f)]
    public LayerMask whatIsTarget;

    [Header("처치 시 시간 증가"), Space(5f)]
    public float increaseTime = 1.0f;

    private ItemSpawner itemSpawner;

    private float attackDelay;
    private float attackSpeed;
    private float searchRadius = 250f;

    private Color hitFlickerColor = Color.red;
    private int hitFlickerCount = 3;
    private float hitFlickerTime = 0.05f;

    private Animator enemyAnimator;
    private Renderer enemyRenderer;

    private LivingEntity targetEntity;
    private NavMeshAgent pathFinder;

    private bool hasTarget { get { return targetEntity != null && !targetEntity.dead; } }

    protected override void OnEnable()
    {
        base.OnEnable();

        if (pathFinder != null)
        {
            pathFinder.enabled = true;
            pathFinder.ResetPath();
        }

        var colliders = GetComponents<Collider>();
        foreach (var collider in colliders)
        {
            collider.enabled = true;
        }

        StartCoroutine(UpdatePath());

        itemSpawner = GetComponent<ItemSpawner>();
        if (itemSpawner != null)
        {
            onDeath += itemSpawner.DropItem;
        }
    }

    private void OnDisable()
    {
        if (pathFinder != null)
        {
            pathFinder.enabled = false;
        }

        if (itemSpawner != null)
        {
            onDeath -= itemSpawner.DropItem;
        }
    }

    private void Awake()
    {
        pathFinder = GetComponent<NavMeshAgent>();
        enemyAnimator = GetComponentInChildren<Animator>();
        enemyRenderer = GetComponentInChildren<Renderer>();

        pathFinder.speed = enemyData.moveSpeed;

        health = enemyData.maxHealth;
        attackDelay = enemyData.attackDelay;
        attackSpeed = enemyData.attackSpeed;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!dead && Time.time >= attackDelay + attackSpeed)
        {
            if (other.tag != "Player")
            {
                return;
            }

            var attackTarget = other.GetComponent<LivingEntity>();
            if (attackTarget != null)
            {
                attackTarget.TakeDamage(enemyData.attackDamage);
            }
            attackDelay = Time.time;
        }
    }

    private IEnumerator UpdatePath()
    {
        while (!dead)
        {
            if (hasTarget)
            {
                pathFinder.isStopped = false;
                pathFinder.SetDestination(targetEntity.transform.position);
            }
            else
            {
                pathFinder.isStopped = true;
                var colliders = Physics.OverlapSphere(transform.position, searchRadius, whatIsTarget);

                for (int i = 0; i < colliders.Length; i++)
                {
                    var livingEntity = colliders[i].GetComponent<LivingEntity>();
                    if (livingEntity != null && !livingEntity.dead)
                    {
                        targetEntity = livingEntity;
                        break;
                    }
                }
            }
            yield return new WaitForSeconds(0.33f);
        }
    }

    public void Setup(float newHealth, float newDamage, float newSpeed, float newDefense, Color skinColor)
    {
        enemyData.maxHealth          = newHealth;
        enemyData.attackDamage       = newDamage;
        enemyData.defense            = newDefense;
        pathFinder.speed             = newSpeed;
        enemyRenderer.material.color = skinColor;
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        if (dead)
        {
            return;
        }
        StartCoroutine(DamagedHitColor());
    }

    private IEnumerator DamagedHitColor()
    {
        for (int i = 0; i < hitFlickerCount; i++)
        {
            enemyRenderer.material.color = hitFlickerColor;
            yield return new WaitForSeconds(hitFlickerTime);

            enemyRenderer.material.color = Color.white;
            yield return new WaitForSeconds(hitFlickerTime);
        }
    }

    public override void Die()
    {
        //base.Die();
        dead = true;
        GameManager.gameTimeLimit += increaseTime;

        var colliders = GetComponents<Collider>();
        foreach (var collider in colliders)
        {
            collider.enabled = false;
        }

        if (pathFinder.isActiveAndEnabled)
        {
            pathFinder.isStopped = true;
            pathFinder.enabled = false;
        }

        enemyAnimator.SetTrigger("Die");
        StartCoroutine(DelayedRelease());
    }

    private IEnumerator DelayedRelease()
    {
        yield return new WaitForSeconds(1f);
        base.DeleyDie();
    }
}