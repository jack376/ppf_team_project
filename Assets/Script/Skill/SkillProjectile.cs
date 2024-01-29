using UnityEngine;

public class SkillProjectile : MonoBehaviour
{
    [HideInInspector] public GameObject originalPrefab;
    [HideInInspector] public GameObject projectileFxPrefab;
    [HideInInspector] public GameObject hitFxPrefab;

    [HideInInspector] public LayerMask targetLayer;
    [HideInInspector] public int pierceCount = 1;

    [HideInInspector] public float speed    = 15f;
    [HideInInspector] public float splash   = 15f;
    [HideInInspector] public float damage   = 100f;
    [HideInInspector] public float lifeTime = 1f;

    [HideInInspector] public float currentSkillDamageEnhance = 1f;

    private PlayerData playerData;

    private float flowTime = 0f;
    private bool isEnable = true;

    private void Awake()
    {
        playerData = GameManager.player.GetComponent<PlayerData>();
    }

    private void OnEnable()
    {
        playerData.OnSkillDamageEnhance += UpdateSkillDamageEnhance;

        flowTime = 0f;
        isEnable = true;
    }

    private void OnDisable()
    {
        if (playerData != null)
        {
            playerData.OnSkillDamageEnhance -= UpdateSkillDamageEnhance;
        }
    }

    private void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;

        var holdPosition = transform.position;
        holdPosition.y = 1f;
        transform.position = holdPosition;

        flowTime += Time.deltaTime;
        if (flowTime >= lifeTime)
        {
            if (isEnable)
            {
                OnHitParticle(hitFxPrefab);
                OnSplashDamage();
                ReleaseProjectileFx();
                isEnable = false;

                return;
            }
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if ((targetLayer & 1 << collision.gameObject.layer) == 0)
        {
            return;
        }

        if (pierceCount > 1)
        {
            pierceCount--;
            OnHitParticle(hitFxPrefab);
            OnSplashDamage();
        }
        else if (pierceCount == 1 && isEnable)
        {
            pierceCount--;
            OnHitParticle(hitFxPrefab);
            OnSplashDamage();
            ReleaseProjectileFx();
            isEnable = false;
        }
    }

    private void OnSplashDamage()
    {
        var hitColliders = Physics.OverlapSphere(transform.position, splash, targetLayer);
        foreach (var hitCollider in hitColliders)
        {
            var target = hitCollider.gameObject.GetComponent<IDamageable>();
            if (target != null)
            {
                target.TakeDamage(damage * currentSkillDamageEnhance);
            }
        }
    }

    private void OnHitParticle(GameObject hitFxPrefab)
    {
        var hitParticleGo = PoolManager.Instance.GetPool(hitFxPrefab).Get();
        hitParticleGo.transform.position = transform.position;
        hitParticleGo.transform.rotation = Quaternion.identity;

        var hitParticle = hitParticleGo.GetComponent<HitParticleHandler>();
        hitParticle.onFinish += ReleaseParticle;

        void ReleaseParticle()
        {
            hitParticle.onFinish -= ReleaseParticle;
            PoolManager.Instance.GetPool(hitFxPrefab).Release(hitParticleGo);
        }
    }

    private void ReleaseProjectileFx()
    {
        PoolManager.Instance.GetPool(projectileFxPrefab).Release(gameObject.transform.GetChild(0).gameObject);
        gameObject.transform.GetChild(0).parent = null;

        PoolManager.Instance.GetPool(originalPrefab).Release(gameObject);
    }

    private void UpdateSkillDamageEnhance(float newSkillDamageEnhance)
    {
        currentSkillDamageEnhance = newSkillDamageEnhance;
    }
}