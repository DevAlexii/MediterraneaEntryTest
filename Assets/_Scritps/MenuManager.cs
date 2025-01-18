using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [Header("Base Button References")]
    [SerializeField]
    private Button StartButton;

    [SerializeField]
    private Button SettingsButton;

    [SerializeField]
    private Button QuitButton;

    [Header("UI Containers References")]
    [SerializeField]
    private GameObject BaseContainer;

    [SerializeField]
    private GameObject SettingContainer;

    [Header("UI SettingReferences")]
    [SerializeField]
    private Slider volumeSlider;

    [SerializeField]
    private Button backButton;

    [SerializeField]
    private TMP_Dropdown qualityDropdown;

    [SerializeField]
    private TMP_Dropdown resolutionDropdown;

    [SerializeField]
    private TMP_Dropdown difficutlyDropdown;

    [SerializeField]
    private Toggle fullScreenToogle;



    private bool settingChanged;

    void Start()
    {
        settingChanged = false;
        ShowBaseContainer(true);
        SetupFunction();
        IntializeSetting();

        AudioManager.Instance.PlayBackground(BackgroundType.Menu);
    }


    private IEnumerator LoadGameplaySceneAsync()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("GameplayScene");
        AudioManager.Instance.PlayBackground(BackgroundType.Game);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    private void OnClickStartBTN()
    {
        if (EventSystem.current != null)
        {
            EventSystem.current.enabled = false;
        }

        StartButton.interactable = false;
        SettingsButton.interactable = false;
        QuitButton.interactable = false;

        StartCoroutine(LoadGameplaySceneAsync());
        AudioManager.Instance.PlayClipSound(ClipType.Button);
    }

    private void OnClickQuitBTN()
    {
        AudioManager.Instance.PlayClipSound(ClipType.Button);
        Application.Quit(0); // 0 => This indicates a proper closure without errors, useful if you want to read logs
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

    private void OnVolumeChange(float value)
    {
        SettingSaver.currentSetting.masterVolume = value;
        AudioListener.volume = value;
        settingChanged = true;
    }

    private void OnClickBackButton()
    {
        AudioManager.Instance.PlayClipSound(ClipType.Button);

        ShowBaseContainer(true);

        if (settingChanged)
        {
            SettingSaver.SaveSettings();
        }
    }

    private void OnQualityChange(int value)
    {
        QualitySettings.SetQualityLevel(value);
        SettingSaver.currentSetting.qualityIndex = value;
        settingChanged = true;
    }

    private void OnResolutionChange(int value)
    {
        ChangeResolution(value);
        SettingSaver.currentSetting.resolutionIndex = value;
        settingChanged = true;
    }

    private void OnDifficultyChange(int value)
    {
        GameManager.ChangeDifficulty(value);
        SettingSaver.currentSetting.difficulty = value;
        settingChanged = true;
    }

    private void SetupFunction()
    {
        if (StartButton && SettingsButton
            && QuitButton && volumeSlider
            && backButton && qualityDropdown
            && resolutionDropdown && difficutlyDropdown
            && fullScreenToogle)
        {
            StartButton.onClick.AddListener(OnClickStartBTN);
            SettingsButton.onClick.AddListener(() =>
            {
                ShowBaseContainer(false);
                AudioManager.Instance.PlayClipSound(ClipType.Button);
            });

            QuitButton.onClick.AddListener(OnClickQuitBTN);
            volumeSlider.onValueChanged.AddListener(OnVolumeChange);
            backButton.onClick.AddListener(OnClickBackButton);
            qualityDropdown.onValueChanged.AddListener(OnQualityChange);
            resolutionDropdown.onValueChanged.AddListener(OnResolutionChange);
            difficutlyDropdown.onValueChanged.AddListener(OnDifficultyChange);
            fullScreenToogle.onValueChanged.AddListener((bool value) => 
            { 
                SettingSaver.currentSetting.fullscreen = value;
                Screen.fullScreen = value;
            });
            return;
        }
        Debug.LogError("Error during buttons function bindingfs : references are not properly set");
    }

    private void IntializeSetting()
    {
        SettingData setting = SettingSaver.currentSetting;

        AudioListener.volume = setting.masterVolume;
        volumeSlider.value = setting.masterVolume;

        QualitySettings.SetQualityLevel(setting.qualityIndex);
        qualityDropdown.value = setting.qualityIndex;

        difficutlyDropdown.value = setting.difficulty;
        GameManager.ChangeDifficulty(setting.difficulty);

        fullScreenToogle.isOn = setting.fullscreen;

        if (setting.resolutionIndex >= 0)
        {
            UpdateResolutionsDropdown(setting.resolutionIndex);
        }
        else
        {
            SetMaxResolution();
        }
    }

    void UpdateResolutionsDropdown(int selectedResolution)
    {
        Resolution[] resolutions = Screen.resolutions;
        List<string> resolutionOptions = new List<string>();

        foreach (Resolution resolution in resolutions)
        {
            string resolutionString = resolution.width + " x " + resolution.height;
            resolutionOptions.Add(resolutionString);
        }

        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(resolutionOptions);

        ChangeResolution(selectedResolution);
        resolutionDropdown.value = selectedResolution;
    }

    private void SetMaxResolution()
    {
        int maxIndexRes = Screen.resolutions.Length - 1;
        ChangeResolution(maxIndexRes);
        UpdateResolutionsDropdown(maxIndexRes);
    }

    private void ChangeResolution(int selectedResolution)
    {
        Resolution maxResolution = Screen.resolutions[selectedResolution];
        Screen.SetResolution(maxResolution.width, maxResolution.height, SettingSaver.currentSetting.fullscreen);
    }
}
