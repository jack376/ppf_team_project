using System;
using UnityEngine;

public class HitParticleHandler : MonoBehaviour
{
    public event Action onFinish;

    private float particleLifetime = 1.5f;
    private float flowTime = 0f;

    private void Update()
    {
        flowTime += Time.deltaTime;
        if (flowTime >= particleLifetime)
        {
            if(onFinish != null)
            {
                onFinish();
                onFinish = null;
            }
            flowTime = 0f;
        }
    }
}