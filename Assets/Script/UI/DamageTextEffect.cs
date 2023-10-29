using UnityEngine;
using TMPro;

public class DamageTextEffect : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float fadeSpeed = 1f;
    public float lifeTime = 1f;

    private TextMeshPro textMeshPro;
    private Color initialColor;
    private float flowTime;

    private void Awake()
    {
        textMeshPro = GetComponent<TextMeshPro>();
        initialColor = textMeshPro.color;
    }

    private void OnEnable()
    {
        flowTime = 0f;
        textMeshPro.color = initialColor;
    }

    private void Update()
    {
        // 위로 움직이기
        transform.position += Vector3.up * moveSpeed * Time.deltaTime;

        // 사라지는 효과
        flowTime += Time.deltaTime;
        if (flowTime >= lifeTime)
        {
            Color currentColor = textMeshPro.color;
            currentColor.a -= fadeSpeed * Time.deltaTime;
            textMeshPro.color = currentColor;

            if (currentColor.a <= 0)
            {
                gameObject.SetActive(false); // or destroy, or return to the object pool
            }
        }
    }
}
