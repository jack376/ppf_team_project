using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public TextMeshProUGUI countdownText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI currentLevelUI;

    public GameObject gameoverUI;
    //public GameObject skillSelectWindowUI;

    public Button[] skillSelectButtons;

    private PlayerWeapon playerWeapon;
    private int[] randomIds = new int[3];
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
        playerWeapon = FindObjectOfType<PlayerWeapon>();


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
        foreach(Button button in skillSelectButtons)
        {
            button.gameObject.SetActive(true);
        }
        Time.timeScale = 0;

        List<GameObject> allSkillPrefabs = SkillManager.Instance.allSkillPrefabs;
        int skillFrontNumber = 1000300;

        HashSet<int> uniqueRandomIds = new HashSet<int>();
        for (int i = 0; i < 3; i++)
        {
            int randomCount;
            do
            {
                randomCount = Random.Range(1, allSkillPrefabs.Count + 1);
            } while (uniqueRandomIds.Contains(randomCount));

            uniqueRandomIds.Add(randomCount);
            randomIds[i] = randomCount + skillFrontNumber;
        }
    }

    public void OnSkillButtonClicked(int buttonIndex)
    {
        playerWeapon.LearnSkill(randomIds[buttonIndex]);

        foreach (Button button in skillSelectButtons)
        {
            button.gameObject.SetActive(false);
        }
        Time.timeScale = 1;

        Debug.Log("Button clicked: " + buttonIndex);
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