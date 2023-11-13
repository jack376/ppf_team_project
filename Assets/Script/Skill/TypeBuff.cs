using Unity.VisualScripting;
using UnityEngine;

public class TypeBuff : ISkill
{
    public void Execute(SkillData skillData, LayerMask targetLayer, Quaternion targetQuaternion, GameObject hitFx, GameObject projectileFx, Vector3 targetPosition)
    {
        OnBuffParticle(hitFx, GameManager.player.transform.position);
    }

    private void OnBuffParticle(GameObject hitFxPrefab, Vector3 originPosition)
    {
        // 히트 파티클 버프 파티클로 대체하기 HitParticleHandler 개조
        // 오오라 파티클 + 셋라이프타임, 버프 능력치 적용
        // 원래 스텟 + 버프 스텟 + 아이템 스텟 = 현재 스텟 -> 이게 적용되도록

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