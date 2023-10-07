using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public enum State
    {
        Ready,
        Empty,
        Reloading
    }

    public State state { get; private set; }

    public Transform fireTransform;

    public ParticleSystem muzzleFlashEffect;

    private LineRenderer bulletLineRenderer;

    private AudioSource gunAudioPlayer;
    public AudioClip shotClip;
    public AudioClip reloadClip;

    public float damage = 25;
    private float fireDistance = 50f;

    public int ammoRemain = 100;
    public int magCapacity = 25;
    public int magAmmo;

    public float timeBetFire = 0.12f;
    public float reloadTime = 1.8f;
    private float lastFireTime;

    public event System.Action onCompleteReload;

    private void Awake()
    {
        gunAudioPlayer = GetComponent<AudioSource>();
        bulletLineRenderer = GetComponent<LineRenderer>();

        bulletLineRenderer.positionCount = 2;
        bulletLineRenderer.enabled = false;
    }

    private void OnEnable()
    {
        state = State.Ready;
        magAmmo = magCapacity;
        lastFireTime = 0f;
    }

    public void Fire()
    {
        if (state == State.Ready && Time.time > lastFireTime + timeBetFire)
        {
            lastFireTime = Time.time;
            Shot();
        }
    }

    private void Shot()
    {
        Vector3 hitPosition = fireTransform.position + fireTransform.forward * fireDistance;
        Ray ray = new Ray(fireTransform.position, fireTransform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, fireDistance))
        {
            IDamageable target = hit.collider.GetComponent<IDamageable>();
            if (target != null)
            {
                target.OnDamage(damage, hit.point, hit.normal);
                hitPosition = hit.point;
            }

            StartCoroutine(ShotEffect(hitPosition));

            --magAmmo;
            if (magAmmo <= 0)
            {
                state = State.Empty;
            }
        }
    }

    private IEnumerator ShotEffect(Vector3 hitPosition)
    {
        muzzleFlashEffect.Play();
        gunAudioPlayer.PlayOneShot(shotClip);

        bulletLineRenderer.SetPosition(0, fireTransform.position);
        bulletLineRenderer.SetPosition(1, hitPosition);
        bulletLineRenderer.enabled = true;

        yield return new WaitForSeconds(0.02f);

        bulletLineRenderer.enabled = false;
    }

    public bool Reload()
    {
        if (state == State.Reloading || ammoRemain <= 0 || magAmmo >= magCapacity)
        {
            return false;
        }

        StartCoroutine(ReloadRoutine());
        return true;
    }

    private IEnumerator ReloadRoutine()
    {
        state = State.Reloading;
        gunAudioPlayer.PlayOneShot(reloadClip);

        yield return new WaitForSeconds(reloadTime);

        var ammoToFill = magCapacity - magAmmo;
        if (ammoRemain < ammoToFill)
        {
            ammoToFill = ammoRemain;
        }
        ammoRemain -= ammoToFill;
        magAmmo += ammoToFill;

        if (onCompleteReload != null)
        {
            onCompleteReload();
        }
        state = State.Ready;
    }
}