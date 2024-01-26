using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public TextMeshProUGUI countdownText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI currentLevelUI;

    public GameObject gameoverUI;
    public List<Button> skillSelectButtons = new List<Button>();

    private PlayerSkill playerSkill;
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

    public void SetActiveGameoverUI(bool active)
    {
        gameoverUI.SetActive(active);
    }

    private IEnumerator StartCountdown()
    {
        int count = 3;

        countdownText.gameObject.SetActive(true);

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
        var allSkillPrefabs = SkillManager.Instance.allSkillPrefabs;

        var uniqueRandomIds = new HashSet<int>();
        for (int i = 0; i < 3; i++)
        {
            int randomCount;

            do { randomCount = Random.Range(0, allSkillPrefabs.Count); }
            while (uniqueRandomIds.Contains(randomCount));

            uniqueRandomIds.Add(randomCount);

            skillSelectButtons[randomCount].gameObject.SetActive(true);
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

        //Debug.Log("Button clicked: " + number);
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