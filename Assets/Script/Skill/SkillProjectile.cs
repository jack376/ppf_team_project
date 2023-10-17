using System.Collections;
using UnityEngine;
using UnityEngine.Pool;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class SkillProjectile : MonoBehaviour
{
    public GameObject hit;
    public GameObject flash;
    public GameObject[] Detached;

    public SkillType type = 0;
    public int isPierceHitPlay = 0;

    public LayerMask targetLayer;
    public LayerMask groundLayer;

    public float speed = 5f;
    public float splash = 1f;
    public float damage = 10f;
    public float lifeTime = 3f;

    public Transform targetTransform;

    private float hoverHeight = 1f;
    private Vector3 direction = Vector3.forward;

    private void Start() // 발사체가 발사될 때 플래시 파티클 재생 
    {
        gameObject.transform.position = GameManager.weapon.transform.position;

        if (flash != null)
        {
            PlayFlashParticle();
        }

        if (type == SkillType.Nova)
        {
            lifeTime = 0f;
        }

        direction = Vector3.forward;

        Invoke("ProjectileRelease", lifeTime);
    }

    private void FixedUpdate()
    {
        if (targetTransform != null)
        {
            direction = (targetTransform.position - transform.position).normalized;
        }

        ProjectileMovement();
        ProjectileHover();
    }

    private void OnCollisionEnter(Collision collision) // 발사체가 타겟 레이어에게 충돌 시 데미지 적용 
    {
        if ((targetLayer & (1 << collision.gameObject.layer)) == 0)
        {
            return;
        }

        if (type != SkillType.Pierce) // 관통형 스킬이 아닐 경우 닿자마자 탄환 삭제
        {
            ProjectileRelease();
        }

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, splash, targetLayer);
        MultiTarget(hitColliders);

        if (isPierceHitPlay == 1)
        {
            PlayFlashParticle();
        }
    }

    private void OnDisable() // OnDestroy() // 발사체가 사라질 때 히트 파티클 재생
    {
        PlayHitParticle();
    }

    public void ProjectileMovement() // 발사체 실시간 움직임
    {
        if (targetTransform != null)
        {
            transform.LookAt(targetTransform.position);
        }
        else
        {
            ProjectileRelease();
        }

        transform.Translate(Vector3.forward * (speed * Time.fixedDeltaTime));
    }

    public void ProjectileHover() // 발사체가 땅에 부딪히지 않도록 y축으로 hoverHeight 만큼 공중 부양 
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, groundLayer))
        {
            Vector3 fixPosition = transform.position;
            fixPosition.y = hit.point.y + hoverHeight;
            transform.position = fixPosition;
        }
    }

    public void MultiTarget(Collider[] hitColliders)
    {
        foreach (Collider collision in hitColliders)
        {
            IDamageable target = collision.gameObject.GetComponent<IDamageable>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }
        }
    }

    public void PlayHitParticle() // 히트 파티클 재생
    {
        if (null != hit)
        {
            GameObject hitInstance = PoolManager.Instance.GetPool(hit.name, hit).Get();
            hitInstance.transform.position = transform.position;
            Invoke("HitRelease", 3f);
        }
    }

    public void PlayFlashParticle() // 플래쉬 파티클 재생
    {
        if (null != flash)
        {
            GameObject flashInstance = PoolManager.Instance.GetPool(flash.name, flash).Get();
            flashInstance.transform.position = transform.position;
            flashInstance.transform.forward = gameObject.transform.forward;
            Invoke("FlashRelease", 3f);
        }
    }

    private void HitRelease()
    {
        if (hit.activeInHierarchy)
        {
            PoolManager.Instance.GetPool(hit.name, hit).Release(hit);
        }    
    }

    private void FlashRelease()
    {
        if (flash.activeInHierarchy)
        {
            PoolManager.Instance.GetPool(flash.name, flash).Release(flash);
        }  
    }

    private void ProjectileRelease()
    {
        if (gameObject.activeInHierarchy)
        {
            PoolManager.Instance.GetPool(gameObject.name, gameObject).Release(gameObject);
        }  
    }
}
