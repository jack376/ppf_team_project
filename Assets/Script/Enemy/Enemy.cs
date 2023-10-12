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

    // 아이템 스포너
    private ItemSpawner itemSpawner;

    // EnemyData에서 넘겨받을 필드
    private float attackDelay;
    private float attackSpeed;
    private float searchRadius = 250f;

    // 피격 시 컬러 변경에 쓰일 필드
    private Color hitFlickerColor = Color.red;
    private int hitFlickerCount = 3;
    private float hitFlickerTime = 0.05f;

    // 애니메이션, 렌더러 컴포넌트 필드
    private Animator enemyAnimator;
    private Renderer enemyRenderer;

    // Nav AI 라이브러리용 필드
    private LivingEntity targetEntity;
    private NavMeshAgent pathFinder;

    // Nav AI에 사용되는 프로퍼티
    private bool hasTarget
    {
        get { return targetEntity != null && !targetEntity.dead; }
    }

    // 컴포넌트 할당 및 필드 초기화
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

    // 플레이어 탐색 코루틴 시작
    private void Start()
    {
        StartCoroutine(UpdatePath());

        itemSpawner = GetComponent<ItemSpawner>();
        if (itemSpawner != null)
        {
            onDeath += itemSpawner.DropItem;
        }
    }

    // 0.25초마다 searchRadius 범위에 죽지 않은 플레이어가 존재하는지 탐색
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

    // 아직 미사용 중, 몬스터의 스탯을 변경할 때 사용되는 메서드
    public void Setup(float newHealth, float newDamage, float newSpeed, float newDefense, Color skinColor)
    {
        enemyData.maxHealth = newHealth;
        enemyData.attackDamage = newDamage;
        enemyData.defense = newDefense;
        pathFinder.speed = newSpeed;
        enemyRenderer.material.color = skinColor;
    }

    // 오버라이드된 LivingEntity 부모 클래스의 TakeDamage 호출
    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        StartCoroutine(DamagedHitColor());
    }

    // 몬스터 피격 시 컬러 점멸 효과
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

    // 죽었을 경우 Nav AI 경로 탐색 중지 및 콜라이더 비활성
    public override void Die()
    {
        base.Die();
        GameManager.gameTimeLimit += 0.1f;

        Collider[] colliders = GetComponents<Collider>();
        foreach (Collider collider in colliders)
        {
            collider.enabled = false;
        }

        pathFinder.isStopped = true;
        pathFinder.enabled = false;

        enemyAnimator.SetTrigger("Die");
    }

    // 몬스터 박스콜라이더와 교차할 시 매 프레임 마다 트리거 작동
    private void OnTriggerStay(Collider other)
    {
        if (!dead && Time.time >= attackDelay + attackSpeed)
        {
            if (other.tag != "Player")
            {
                return;
            }

            LivingEntity attackTarget = other.GetComponent<LivingEntity>();
            if (attackTarget != null)
            {
                attackTarget.TakeDamage(enemyData.attackDamage);
            }
            attackDelay = Time.time;
        }
    }
}