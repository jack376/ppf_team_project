using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : LivingEntity
{
    public Slider healthSlider;

    private PlayerController playerController;
    private Renderer playerRenderer;
    private Animator playerAnimator;

    // 피격 시 컬러 변경에 쓰일 필드
    private Color hitFlickerColor = Color.red;
    private int hitFlickerCount = 3;
    private float hitFlickerTime = 0.05f;

    // Die가 여러번 호출되지 않도록
    private bool hasDied = false;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        playerAnimator = GetComponent<Animator>();
        playerRenderer = GetComponentInChildren<Renderer>();
    }

    private void Update()
    {
        if (!hasDied && 0f >= GameManager.gameTimeLimit)
        {
            Die();
            hasDied = true;
        }
    }

    // 오버라이드된 LivingEntity 부모 클래스의 OnEnable 호출
    protected override void OnEnable()
    {
        base.OnEnable();

        playerController.enabled = true;

        healthSlider.gameObject.SetActive(true);
        healthSlider.minValue = 0f;
        healthSlider.maxValue = health;
        healthSlider.value = health;
    }

    // 오버라이드된 LivingEntity 부모 클래스의 Healing 호출
    public override void Healing(float newHealth)
    {
        base.Healing(newHealth);
        healthSlider.value = health;
    }

    // 오버라이드된 LivingEntity 부모 클래스의 TakeDamage 호출
    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        StartCoroutine(DamagedHitColor());
        healthSlider.value = health;
    }

    // 몬스터 피격 시 컬러 점멸 효과 및 피격 애니메이션 재생
    private IEnumerator DamagedHitColor()
    {
        if (GameManager.isGameover)
        {
            yield break;
        }

        playerAnimator.SetBool("Hit Bool", true);
        for (int i = 0; i < hitFlickerCount; i++)
        {
            playerRenderer.material.color = hitFlickerColor;
            yield return new WaitForSeconds(hitFlickerTime);
            playerAnimator.SetBool("Hit Bool", false);

            playerRenderer.material.color = Color.white;
            yield return new WaitForSeconds(hitFlickerTime);
        }
    }

    // 죽었을 때 부모 클래스 Die 호출, 플레이어 컨트롤러 스크립트 비활성화
    public override void Die()
    {
        base.Die();
        playerController.enabled = false;
        playerAnimator.SetTrigger("Die");
        healthSlider.gameObject.SetActive(false);
    }
}