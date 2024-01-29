using System;
using TMPro;
using UnityEngine;

public class DamageTextHandler : MonoBehaviour
{
    public event Action onDamageText;

    public float moveSpeed = 2f;
    public float fadeSpeed = 0.5f;

    private float lifeTime = 0.5f;
    private float flowTime = 0f;

    private TextMeshPro textMeshPro;
    private Color initialColor;

    private void Awake()
    {
        textMeshPro = GetComponentInChildren<TextMeshPro>();
        initialColor = textMeshPro.color;
    }

    private void OnEnable()
    {
        flowTime = 0f;
        textMeshPro.color = initialColor;
    }

    private void Update()
    {
        transform.position += Vector3.up * moveSpeed * Time.deltaTime;
        transform.forward = Camera.main.transform.forward;

        flowTime += Time.deltaTime;
        if (flowTime >= lifeTime)
        {
            var currentColor = textMeshPro.color;
            currentColor.a -= fadeSpeed * Time.deltaTime;

            textMeshPro.color = currentColor;

            if (currentColor.a <= 0)
            {
                onDamageText();
                onDamageText = null;
            }
        }
    }
}