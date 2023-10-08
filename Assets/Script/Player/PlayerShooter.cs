using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    public Gun gun;
    public Transform gunPivot;

    //private PlayerInput playerInput;
    private Animator playerAnimator;

    private void Start()
    {
        //playerInput = GetComponent<PlayerInput>();
        playerAnimator = GetComponentInChildren<Animator>();
        gun.onCompleteReload += () => UpdateUI();
    }

    private void OnDisable()
    {
        gun.gameObject.SetActive(false);
    }

    private void Update()
    {
        /*
        if (playerInput.touch)
        {
            gun.Fire();
            UpdateUI();
        }
        */
    }

    private void UpdateUI()
    {
        UIManager.instance.UpdateAmmoText(gun.magAmmo, gun.ammoRemain);
    }
}