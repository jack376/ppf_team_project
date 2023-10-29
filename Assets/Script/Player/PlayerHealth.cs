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
    private Renderer playerRenderer;
    private Animator playerAnimator;
    private PlayerData playerData;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        playerAnimator   = GetComponent<Animator>();
        playerRenderer   = GetComponentInChildren<Renderer>();
        playerData       = GetComponent<PlayerData>();
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

        health = playerData.maxHealth;
        playerController.enabled = true;

        healthSlider.gameObject.SetActive(true);
        healthSlider.minValue = 0f;
        healthSlider.maxValue = playerData.maxHealth;
        healthSlider.value    = playerData.maxHealth;
    }

    public override void Healing(float newHealth)
    {
        base.Healing(newHealth);
        healthSlider.value = health;
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
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
            playerRenderer.material.color = hitFlickerColor;
            yield return new WaitForSeconds(hitFlickerTime);
            playerAnimator.SetBool("Hit Bool", false);

            playerRenderer.material.color = Color.white;
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