using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : LivingEntity
{
    public EnemyData enemyData;
    public LayerMask whatIsTarget;

    private float searchRadius = 50f;

    private Color hitFlickerColor = Color.red;
    private int hitFlickerCount = 3;
    private float hitFlickerTime = 0.05f;

    private Animator enemyAnimator;
    private Renderer enemyRenderer;

    private LivingEntity targetEntity;
    private NavMeshAgent pathFinder;

    private bool hasTarget
    {
        get { return targetEntity != null && !targetEntity.dead; }
    }

    private void Awake() 
    {
        pathFinder = GetComponent<NavMeshAgent>();
        enemyAnimator = GetComponentInChildren<Animator>();
        enemyRenderer = GetComponentInChildren<Renderer>();

        pathFinder.speed = enemyData.moveSpeed;
        health = enemyData.maxHealth;
    }

    public void Setup(float newHealth, float newDamage, float newSpeed, float newDefense, Color skinColor)
    {
        enemyData.maxHealth = newHealth;
        enemyData.attackDamage = newDamage;
        enemyData.defense = newDefense;
        pathFinder.speed = newSpeed;
        enemyRenderer.material.color = skinColor;
    }

    private void Start()
    {
        StartCoroutine(UpdatePath());
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
                Collider[] colliders = Physics.OverlapSphere(transform.position, searchRadius, whatIsTarget);

                for (int i = 0; i < colliders.Length; i++)
                {
                    LivingEntity livingEntity = colliders[i].GetComponent<LivingEntity>();
                    if (livingEntity != null && !livingEntity.dead)
                    {
                        targetEntity = livingEntity;
                        break;
                    }
                }
            }
            yield return new WaitForSeconds(0.25f);
        }
    }

    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        base.OnDamage(damage, hitPoint, hitNormal);
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
        base.Die();

        Collider[] colls = GetComponents<Collider>();
        foreach (Collider coll in colls)
        {
            coll.enabled = false;
        }

        pathFinder.isStopped = true;
        pathFinder.enabled = false;

        enemyAnimator.SetTrigger("Die");
    }

    private void OnTriggerStay(Collider other)
    {
        if (!dead && Time.time >= enemyData.attackDelay + enemyData.attackSpeed)
        {
            LivingEntity attackTarget = other.GetComponent<LivingEntity>();
            if (attackTarget != null)
            {
                Vector3 hitPoint = other.ClosestPoint(transform.position);
                Vector3 hitNormal = transform.position - other.transform.position;

                attackTarget.OnDamage(enemyData.attackDamage, hitPoint, hitNormal);
                enemyData.attackDelay = Time.time;
            }
        }
    }
}