using System;
using UnityEngine;

public class Gem : MonoBehaviour, IItem
{
    public event Action onItemRelease;
    public float expPoint = 25f;

    public void Use(GameObject target)
    {
        PlayerExp playerExp = target.GetComponent<PlayerExp>();

        if (playerExp != null)
        {
            playerExp.GetExp(expPoint);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Use(other.gameObject);
            onItemRelease();
        }
    }
}