using Cinemachine;
using System.Collections;
using TMPro;
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

    [Header("데미지 텍스트 프리팹"), Space(5f)]
    public GameObject damageTextPrefab;

    private ItemSpawner itemSpawner;

    private float attackDelay;
    private float attackSpeed;
    private float searchRadius = 250f;
    private float delayDieTime = 1.5f;

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
        pathFinder    = GetComponent<NavMeshAgent>();
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

    public void SetEnemyStat(float newHealth, float newDamage, float newSpeed, float newDefense, Color skinColor)
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
        ShowDamageText(damageTextPrefab, damage);

        if (dead)
        {
            return;
        }

        StartCoroutine(DamagedHitColor());
    }

    private void ShowDamageText(GameObject damageTextPrefab, float damage)
    {
        var damageTextGo = PoolManager.Instance.GetPool(damageTextPrefab).Get();
        damageTextGo.transform.position = transform.position;

        var damageValue = damageTextGo.GetComponentInChildren<TextMeshPro>();
        damageValue.text = damage.ToString();

        var damageText = damageTextGo.GetComponent<DamageTextHandler>();
        damageText.onDamageText += ReleaseDamageText;

        void ReleaseDamageText()
        {
            damageText.onDamageText -= ReleaseDamageText;
            PoolManager.Instance.GetPool(damageTextPrefab).Release(damageTextGo);
        }
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
        yield return new WaitForSeconds(delayDieTime);
        base.DeleyDie();
    }
}