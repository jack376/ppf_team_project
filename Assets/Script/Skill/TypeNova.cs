using UnityEngine;

public class TypeNova : ISkill
{
    public void Execute(SkillData skillData, LayerMask targetLayer, Quaternion targetQuaternion, GameObject hitFx, GameObject projectileFx, Vector3 targetPosition)
    {
        OnAreaDamage(skillData, targetLayer, GameManager.player.transform.position, hitFx);
        OnHitParticle(hitFx, GameManager.player.transform.position);
    }

    private void OnAreaDamage(SkillData skillData, LayerMask targetLayer, Vector3 originPosition, GameObject hitFx)
    {
        var hitColliders = Physics.OverlapSphere(originPosition, skillData.areaRadius, targetLayer);
        foreach (var hitCollider in hitColliders)
        {
            var target = hitCollider.gameObject.GetComponent<IDamageable>();
            if (target != null)
            {
                target.TakeDamage(skillData.damage);
            }
        }
    }

    private void OnHitParticle(GameObject hitFxPrefab, Vector3 originPosition)
    {
        var hitParticleGo = PoolManager.Instance.GetPool(hitFxPrefab).Get();
        hitParticleGo.transform.position = originPosition;
        hitParticleGo.transform.rotation = Quaternion.identity;

        var hitParticle = hitParticleGo.GetComponent<HitParticleHandler>();
        hitParticle.onFinish += ReleaseParticle;

        void ReleaseParticle()
        {
            hitParticle.onFinish -= ReleaseParticle;
            PoolManager.Instance.GetPool(hitFxPrefab).Release(hitParticleGo);
        }
    }
}
