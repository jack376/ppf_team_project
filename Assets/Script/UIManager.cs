using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class UIManager : MonoBehaviour 
{
    public static UIManager Instance { get; private set; }

    public TextMeshProUGUI countdownText;
    public GameObject gameoverUI;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        StartCoroutine(StartCountdown());
    }

    private void Update()
    {
        if (GameManager.isGameover && Input.GetKeyDown(KeyCode.R))
        {
            GameRestart();
        }
    }

    // 게임 오버 시 게임 오버 UI 활성화
    public void SetActiveGameoverUI(bool active) 
    {
        gameoverUI.SetActive(active);
    }

    // 게임 재시작 시 씬 다시 로드
    public void GameRestart() 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // 카운트 다운 코루틴
    private IEnumerator StartCountdown()
    {
        countdownText.gameObject.SetActive(true);
        int count = 3;

        while (count > 0)
        {
            countdownText.text = count.ToString();
            yield return new WaitForSeconds(1f);
            count--;
        }

        countdownText.text = "GO!";
        yield return new WaitForSeconds(1f);
        countdownText.gameObject.SetActive(false);
    }
}