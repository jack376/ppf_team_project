using UnityEngine;

public class OptionsMenu : MonoBehaviour
{
    public GameObject optionsWindow;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            optionsWindow.SetActive(!optionsWindow.activeSelf);
        }
    }
}