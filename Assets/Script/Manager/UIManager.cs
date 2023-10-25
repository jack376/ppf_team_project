using UnityEngine;
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
    public Button[] skillSelectButtons = new Button[5];

    private PlayerSkill playerSkill;
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
        playerSkill = FindObjectOfType<PlayerSkill>();

        StartCoroutine(StartCountdown());
    }

    private void Update()
    {
        if (GameManager.isGameover && Input.GetKeyDown(KeyCode.R))
        {
            GameManager.Instance.GameRestart();
        }
    }

    public void SetActiveGameoverUI(bool active)
    {
        gameoverUI.SetActive(active);
    }

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
        List<GameObject> allSkillPrefabs = SkillManager.Instance.allSkillPrefabs;

        HashSet<int> uniqueRandomIds = new HashSet<int>();
        for (int i = 0; i < 3; i++)
        {
            int randomCount;
            do
            {
                randomCount = Random.Range(1, allSkillPrefabs.Count + 1);
            } while (uniqueRandomIds.Contains(randomCount));

            uniqueRandomIds.Add(randomCount);
            randomIds[i] = randomCount + 10000000;
            Debug.Log(randomIds[i]);
            Debug.Log(randomCount + 10000000);

            switch (randomIds[i])
            {
                case 10000001: skillSelectButtons[0].gameObject.SetActive(true); break;
                case 10000002: skillSelectButtons[1].gameObject.SetActive(true); break;
                case 10000003: skillSelectButtons[2].gameObject.SetActive(true); break;
                case 10000004: skillSelectButtons[3].gameObject.SetActive(true); break;
                case 10000005: skillSelectButtons[4].gameObject.SetActive(true); break;
            }
        }

        Time.timeScale = 0;
    }

    public void OnSkillButtonClicked(int number)
    {
        playerSkill.LearnSkill(number);
        foreach (var button in skillSelectButtons)
        {
            button.gameObject.SetActive(false);
        }

        Time.timeScale = 1;

        Debug.Log("Button clicked: " + number);
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