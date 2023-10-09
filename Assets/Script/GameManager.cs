using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public static bool isGameover { get; private set; } = true;

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

    // 게임 시작 시 PlayerHealth 클래스를 찾고 onDeath 이벤트 메서드 구독, onDeath 메서드 실행 시 EndGame 실행
    private void Start()
    {
        FindObjectOfType<PlayerHealth>().onDeath += EndGame;
        StartCoroutine(StartGame());
    }

    // 3초 뒤에 게임 시작 isGameover 불리언 값 false 변경
    private IEnumerator StartGame() 
    {
        yield return new WaitForSeconds(3f);
        isGameover = false;
    }

    // 죽으면 게임 오버, isGameover 불리언 값 true 변경
    public void EndGame()
    {
        isGameover = true;
        UIManager.Instance.SetActiveGameoverUI(true);
    }
}