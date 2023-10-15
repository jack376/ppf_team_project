using UnityEngine;

public class SkillProjectile : MonoBehaviour
{
    internal SkillProjectile skillProjectile;

    internal GameObject hit;
    internal GameObject flash;
    internal GameObject[] Detached;

    internal SkillType type = 0;
    internal int isPierceHitPlay = 0;

    internal LayerMask targetLayer;
    internal LayerMask groundLayer;

    internal float speed = 5f;
    internal float splash = 1f;
    internal float damage = 10f;
    internal float lifeTime = 3f;

    private float hoverHeight = 1f;

    private void Start() // 발사체가 발사될 때 플래시 파티클 재생 
    {
        if (flash != null)
        {
            PlayFlashParticle();
        }

        if (type == SkillType.Nova)
        {
            lifeTime = 0f;
        }

        Destroy(gameObject, lifeTime);
    }

    private void FixedUpdate()
    {
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
            Destroy(gameObject);
        }

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, splash, targetLayer);
        MultiTarget(hitColliders);

        if (isPierceHitPlay == 1)
        {
            PlayFlashParticle();
        }

        PlayHitParticle();
    }

    private void OnDestroy() // 발사체가 사라질 때 히트 파티클 재생
    {
        PlayHitParticle();
    }

    public void ProjectileMovement() // 발사체 실시간 움직임
    {
        transform.Translate(Vector3.forward * (speed * Time.fixedDeltaTime));
    }

    public void ProjectileHover() // 발사체가 땅에 부딪히지 않도록 y축으로 hoverHeight 만큼 공중 부양 
    {

        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, groundLayer))
        {
            Vector3 fixPosition = hit.point;
            fixPosition.y += hoverHeight;
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
            GameObject hitInstance = Instantiate(hit, transform.position, Quaternion.identity);

            ParticleSystem hitParticle = hitInstance.GetComponent<ParticleSystem>();
            Destroy(hitInstance, hitParticle.main.duration);
        }
    }

    public void PlayFlashParticle() // 플래쉬 파티클 재생
    {
        if (null != flash)
        {
            GameObject flashInstance = Instantiate(flash, transform.position, Quaternion.identity);
            flashInstance.transform.forward = gameObject.transform.forward;

            ParticleSystem flashParticle = flashInstance.GetComponent<ParticleSystem>();
            Destroy(flashInstance, flashParticle.main.duration);
        }
    }
}
