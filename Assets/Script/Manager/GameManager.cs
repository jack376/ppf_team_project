using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public static bool isGameover { get; private set; } = true;
    public static GameObject weapon;

    public float timeLimit = 90f;
    public static float gameTimeLimit = 90f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "InGameScene")
        {
            weapon = GameObject.FindWithTag("Weapon");

            gameTimeLimit = timeLimit;
            FindObjectOfType<PlayerHealth>().onDeath += EndGame;
            StartCoroutine(StartGame());
        }
    }

    private IEnumerator StartGame() 
    {
        yield return new WaitForSeconds(3f);
        isGameover = false;
    }

    public void EndGame()
    {
        isGameover = true;
        UIManager.Instance.SetActiveGameoverUI(true);

        AutoSaveData();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void GameRestart()
    {
        SceneManager.LoadScene("LobbyScene");
        isGameover = true;
    }

    public void AutoSaveData()
    {
        InventoryManager.Instance.SaveInventoryDataToCSV("Assets/Resources/InventorySaveData.csv");
    }
}