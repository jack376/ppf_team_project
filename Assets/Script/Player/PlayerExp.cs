using UnityEngine;
using UnityEngine.UI;

public class PlayerExp : MonoBehaviour
{
    public Slider expSlider;

    [Header("레벨 - 현재, 최소, 최대 / 필요 경험치, 필경 증가 계수"), Space(5f)]
    public int currentLevel = 1;
    public int minLevel = 1;
    public int maxLevel = 99;

    public float currentExp = 0f;
    public float needExp = 99f;
    public float expRatio = 1f;

    public delegate void LevelUpAction(int newLevel);
    public event LevelUpAction OnLevelUp;

    private void Start()
    {
        currentLevel = 1;
        UIManager.Instance.currentLevelUI.text = string.Format("LV {0}", currentLevel);

        expSlider.minValue = 0f;
        expSlider.maxValue = needExp;
        expSlider.value = currentExp;
    }

    public void GetExp(float amount)
    {
        currentExp += amount;
        expSlider.value = currentExp;

        if (currentExp >= needExp)
        {
            if (currentLevel < maxLevel)
            {
                LevelUp();
                UIManager.Instance.currentLevelUI.text = string.Format("LV {0}", currentLevel);
            }
        }
    }

    private void LevelUp()
    {
        currentLevel++;

        currentExp = 0f;
        expSlider.value = currentExp;
        
        needExp *= expRatio;
        expSlider.maxValue = needExp;

        OnLevelUp?.Invoke(currentLevel);

        UIManager.Instance.OpenSkillSelectWindow();
    }
}
