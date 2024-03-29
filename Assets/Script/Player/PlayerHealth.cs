using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : LivingEntity
{
    public Slider healthSlider;

    private Color hitFlickerColor = Color.red;
    private int   hitFlickerCount = 3;
    private float hitFlickerTime  = 0.05f;

    private bool hasDied = false;

    private PlayerController playerController;
    private Animator playerAnimator;
    private PlayerData playerData;
    private Renderer[] playerRenderers;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        playerAnimator   = GetComponent<Animator>();
        playerData       = GetComponent<PlayerData>();
        playerRenderers  = GetComponentsInChildren<Renderer>();
    }

    private void Start()
    {
        playerController.enabled = true;
        healthSlider.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (!hasDied && 0f >= GameManager.gameTimeLimit)
        {
            Die();
            hasDied = true;
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        health = playerData.fianlMaxHealth;
        playerController.enabled = true;

        healthSlider.gameObject.SetActive(true);
        healthSlider.minValue = 0f;
        healthSlider.maxValue = playerData.fianlMaxHealth;
        healthSlider.value    = playerData.fianlMaxHealth;
    }

    public override void Healing(float newHealth)
    {
        base.Healing(newHealth);
        healthSlider.value = health;
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(Mathf.Max(damage - playerData.finalArmorPoint, 0));
        StartCoroutine(DamagedHitColor());
        healthSlider.value = health;
    }

    private IEnumerator DamagedHitColor()
    {
        if (GameManager.isGameover)
        {
            yield break;
        }

        playerAnimator.SetBool("Hit Bool", true);
        for (int i = 0; i < hitFlickerCount; i++)
        {
            foreach (var renderer in playerRenderers)
            {
                renderer.material.color = hitFlickerColor;
            }
            yield return new WaitForSeconds(hitFlickerTime);

            playerAnimator.SetBool("Hit Bool", false);
            foreach (var renderer in playerRenderers)
            {
                renderer.material.color = Color.white;
            }
            yield return new WaitForSeconds(hitFlickerTime);
        }
    }

    public override void Die()
    {
        base.Die();
        playerController.enabled = false;
        playerAnimator.SetTrigger("Die");
        healthSlider.gameObject.SetActive(false);
    }
}