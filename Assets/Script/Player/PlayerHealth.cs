using System.Collections;
using UnityEngine;

public class PlayerHealth : LivingEntity
{
    private PlayerController playerController;
    private Renderer playerRenderer;

    // 피격 시 컬러 변경에 쓰일 필드
    private Color hitFlickerColor = Color.red;
    private int hitFlickerCount = 3;
    private float hitFlickerTime = 0.05f;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        playerRenderer = GetComponentInChildren<Renderer>();
    }

    // 오버라이드된 LivingEntity 부모 클래스의 OnEnable 호출
    protected override void OnEnable()
    {
        base.OnEnable();
        playerController.enabled = true;
    }

    // 오버라이드된 LivingEntity 부모 클래스의 Healing 호출
    public override void Healing(float newHealth)
    {
        base.Healing(newHealth);
    }

    // 오버라이드된 LivingEntity 부모 클래스의 OnDamage 호출
    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitDirection)
    {
        base.OnDamage(damage, hitPoint, hitDirection);
        StartCoroutine(DamagedHitColor());
    }

    // 몬스터 피격 시 컬러 점멸 효과
    private IEnumerator DamagedHitColor()
    {
        for (int i = 0; i < hitFlickerCount; i++)
        {
            playerRenderer.material.color = hitFlickerColor;
            yield return new WaitForSeconds(hitFlickerTime);

            playerRenderer.material.color = Color.white;
            yield return new WaitForSeconds(hitFlickerTime);
        }
    }

    // 죽었을 때 부모 클래스 Die 호출, 플레이어 컨트롤러 스크립트 비활성화
    public override void Die()
    {
        base.Die();
        playerController.enabled = false;
    }

    // 아이템 습득, 아직 미구현
    private void OnTriggerEnter(Collider other)
    {
        IItem item = other.GetComponent<IItem>();
        if (item != null)
        {
            item.Use(gameObject);
        }
    }
}