using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    //Buttons References
    [SerializeField]
    private Button StartButton;

    [SerializeField]
    private Button SettingsButton;

    [SerializeField]
    private Button QuitButton;

    //UI Containers References
    [SerializeField]
    private GameObject BaseContainer;

    [SerializeField]
    private GameObject SettingContainer;


    void Start()
    {
        ShowBaseContainer(true);
        SetupButtons();
    }


    private void OnClickStartBTN()
    {
        //...async load
    }

    private void OnClickQuitBTN()
    {
        Application.Quit(0); // 0 => This indicates a proper closure without errors, useful if you want to read logs
    }

    private void OnClickSettingBTN()
    {
        ShowBaseContainer(false);
    }

    private void ShowBaseContainer(bool visible)
    {
        if (BaseContainer && SettingContainer)
        {
            BaseContainer.SetActive(visible);
            SettingContainer.SetActive(!visible);
            return;
        }
        Debug.LogError("Error on clicking the Settings button: references are not properly set");
    }

    private void SetupButtons()
    {
        if (StartButton && SettingsButton && QuitButton)
        {
            StartButton.onClick.AddListener(OnClickStartBTN);
            SettingsButton.onClick.AddListener(OnClickSettingBTN);
            QuitButton.onClick.AddListener(OnClickQuitBTN);
            return;
        }
        Debug.LogError("Error during buttons function bindingfs : references are not properly set");
    }
}
