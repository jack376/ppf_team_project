using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour 
{
    public static UIManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<UIManager>();
            }

            return m_instance;
        }
    }

    private static UIManager m_instance;

    public Image fadeOutImage;
    public float fadeDuration = 1f;

    public Text ammoText;
    public Text scoreText;
    public GameObject gameoverUI;

    public void StartFadeOut()
    {
        StartCoroutine(FadeImageOut());
    }

    IEnumerator FadeImageOut()
    {
        Color imageColor = fadeOutImage.color;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            fadeOutImage.color = new Color(imageColor.r, imageColor.g, imageColor.b, alpha);
            yield return null;
        }

        fadeOutImage.color = new Color(imageColor.r, imageColor.g, imageColor.b, 1f); 
    }

    public void UpdateAmmoText(int magAmmo, int remainAmmo) 
    {
        ammoText.text = magAmmo + "/" + remainAmmo;
    }

    public void UpdateScoreText(int newScore) 
    {
        scoreText.text = "Score : " + newScore;
    }

    public void SetActiveGameoverUI(bool active) 
    {
        gameoverUI.SetActive(active);
        if (active)
        {
            StartFadeOut();
        }
    }

    public void GameRestart() 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}