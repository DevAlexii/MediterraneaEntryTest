using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    public static GameUIManager Instance;

    [SerializeField]
    private TMP_Text timerTXT;
    [SerializeField]
    private GameObject gameOverPopup;
    [SerializeField]
    private Button resetBTN;
    [SerializeField]
    private Button menuBTN;
    [SerializeField]
    private Image collectedItemsSlider;
    [SerializeField]
    private TMP_Text collectableItemsText;
    [SerializeField]
    private GameObject starsContainer;

    private float successRatio;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        gameOverPopup.SetActive(false);
        GameManager.OnGameOverCallback += () => { if (gameOverPopup) { gameOverPopup.SetActive(true); UpdateStarsAlpha(); } };
        if (resetBTN)
        {
            resetBTN.onClick.AddListener(() =>
            {
                if (gameOverPopup) { gameOverPopup.SetActive(false); }
                GameManager.OnRestartGameCallback?.Invoke();
                AudioManager.Instance.PlayClipSound(ClipType.Button);
            });
        }
        if (menuBTN)
        {
            menuBTN.onClick.AddListener(() =>
            {
                StartCoroutine(LoadMenuSceneAsync());
                AudioManager.Instance.PlayClipSound(ClipType.Button);
            });
        }
    }
    private IEnumerator LoadMenuSceneAsync()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("MenuScene");
        AudioManager.Instance.PlayBackground(BackgroundType.Menu);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
    public void UpdateTimerUI(int timer)
    {
        if (timerTXT)
        {
            timerTXT.text = "Timer:  " + timer;
        }
    }
    public void UpdateItems(Int32 current, Int32 max)
    {
        successRatio = (float)current / max;
        collectedItemsSlider.fillAmount = successRatio;
        collectableItemsText.text = current + "/" + max;
    }
    private void UpdateStarsAlpha()
    {
        if (starsContainer == null) return;

        Image[] starImages = starsContainer.GetComponentsInChildren<Image>();


        if (successRatio < 0.25f)
        {
            SetAlpha(starImages[0], 0.3f);
            SetAlpha(starImages[1], 0.3f);
            SetAlpha(starImages[2], 0.3f);
        }
        else if (successRatio < 0.5f)
        {
            SetAlpha(starImages[0], 1f);
            SetAlpha(starImages[1], 0.3f);
            SetAlpha(starImages[2], 0.3f);
        }
        else if (successRatio < 1f)
        {
            SetAlpha(starImages[0], 1f);
            SetAlpha(starImages[1], 1f);
            SetAlpha(starImages[2], 0.3f);
        }
        else
        {
            SetAlpha(starImages[0], 1f);
            SetAlpha(starImages[1], 1f);
            SetAlpha(starImages[2], 1f);
        }
    }

    private void SetAlpha(Image image, float alpha)
    {
        if (image != null)
        {
            Color color = image.color;
            color.a = alpha;
            image.color = color;
        }
    }
}
