using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public TextMeshProUGUI countdownText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI currentLevelUI;

    public GameObject gameoverUI;
    public GameObject skillSelectWindowUI;

    private bool paused = false;

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
            GameManager.Instance.GameRestart();
        }
    }

    // 게임 오버 시 게임 오버 UI 활성화
    public void SetActiveGameoverUI(bool active)
    {
        gameoverUI.SetActive(active);
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
        StartCoroutine(UpdateTimer());
    }

    // 게임 시간 코루틴
    private IEnumerator UpdateTimer()
    {
        while (!GameManager.isGameover)
        {
            GameManager.gameTimeLimit -= Time.deltaTime;

            int minutes = Mathf.FloorToInt(GameManager.gameTimeLimit / 60f);
            int seconds = Mathf.FloorToInt(GameManager.gameTimeLimit % 60f);

            if (0 > seconds)
            {
                minutes = 0;
                seconds = 0;
            }

            timerText.text = string.Format("{0:00} : {1:00}", minutes, seconds);
            yield return new WaitForEndOfFrame();
        }
    }

    public void OpenSkillSelectWindow()
    {
        skillSelectWindowUI.SetActive(true);
        Time.timeScale = 0;
    }

    public void CloseSkillSelectWindow()
    {
        skillSelectWindowUI.SetActive(false);
        Time.timeScale = 1;
    }

    public void PausedButton()
    {
        if(!paused)
        {
            Time.timeScale = 0;
            paused = true;
        }
        else
        {
            Time.timeScale = 1;
            paused = false;
        }
    }
}