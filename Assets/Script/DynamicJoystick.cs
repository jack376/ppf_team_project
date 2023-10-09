using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DynamicJoystick : Joystick
{
    public float MoveThreshold { get { return moveThreshold; } set { moveThreshold = Mathf.Abs(value); } }

    [SerializeField] private float moveThreshold = 1;

    // GUI PRO LED 점등용 필드 - 하이어라키 상에서 Play_Joystick 비활성화 하지 말 것
    private GameObject topImageR;
    private GameObject topImageL;
    private GameObject botImageR;
    private GameObject botImageL;

    private float threshold = 0.05f;

    private void Awake()
    {
        topImageR = GameObject.Find("Move_Focus_tl");
        topImageL = GameObject.Find("Move_Focus_tr");
        botImageR = GameObject.Find("Move_Focus_bl");
        botImageL = GameObject.Find("Move_Focus_br");
    }

    protected override void Start()
    {
        MoveThreshold = moveThreshold;
        base.Start();
        background.gameObject.SetActive(false);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
        background.gameObject.SetActive(true);
        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        background.gameObject.SetActive(false);
        base.OnPointerUp(eventData);
    }
    
    protected override void HandleInput(float magnitude, Vector2 normalised, Vector2 radius, Camera cam)
    {
        // 조이스틱 위치 고정
        /* if (magnitude > moveThreshold)
        {
            Vector2 difference = normalised * (magnitude - moveThreshold) * radius;
            background.anchoredPosition += difference;
        } */

        // 기존 조이스틱 코드에 GUI PRO LED 기능 추가
        topImageL.SetActive(normalised.x >  threshold && normalised.y >  threshold ? true : false);
        topImageR.SetActive(normalised.x < -threshold && normalised.y >  threshold ? true : false);
        botImageR.SetActive(normalised.x < -threshold && normalised.y < -threshold ? true : false);
        botImageL.SetActive(normalised.x >  threshold && normalised.y < -threshold ? true : false);

        base.HandleInput(magnitude, normalised, radius, cam);
    }
}