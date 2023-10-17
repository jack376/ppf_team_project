using System.Collections;
using TMPro;
using UnityEngine;

public class BlinkText : MonoBehaviour
{
    public TextMeshProUGUI blinkText;
    public float blinkDuration = 0.5f;

    private void Start()
    {
        StartCoroutine(Blink());
    }

    private IEnumerator Blink()
    {
        while (true)
        {
            yield return FadeText(0f, 1f, blinkDuration / 2);
            yield return FadeText(1f, 0f, blinkDuration / 2);
        }
    }

    private IEnumerator FadeText(float startAlpha, float endAlpha, float duration)
    {
        float elapsed = 0f;
        Color color = blinkText.color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            color.a = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);
            blinkText.color = color;
            yield return null;
        }

        color.a = endAlpha;
        blinkText.color = color;
    }
}
