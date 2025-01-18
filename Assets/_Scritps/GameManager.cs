using System;
using UnityEngine;

public enum Difficulty
{
    Easy,
    Medium,
    Hard
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game Setup")]
    [SerializeField]
    private float gameCountDown = 10;
    private float currentCountDown;
    [SerializeField]
    private Int32 collectableItems = 20;
    private Int32 collectedItems;

    //Actions
    public static Action OnGameOverCallback;
    public static Action OnRestartGameCallback;
    public static Action<Int32> OnStartGameCallback;


    private Difficulty difficulty;

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
        OnRestartGameCallback += ResetData;
        ResetData();
        OnStartGameCallback?.Invoke(collectableItems);
    }

    private void ResetData()
    {
        currentCountDown = gameCountDown;
        collectedItems = 0;
        GameUIManager.Instance.UpdateItems(collectedItems, collectableItems);
    }

    private void DecrementCountDown()
    {
        currentCountDown = Mathf.Clamp(currentCountDown - Time.deltaTime, 0, gameCountDown);
        GameUIManager.Instance.UpdateTimerUI((int)currentCountDown);

        if (currentCountDown <= 0)
        {
            OnGameOverCallback?.Invoke();
        }
    }

    private void Update()
    {
        DecrementCountDown();
    }

    public void IncrementCollectedItems()
    {
        collectedItems++;
        GameUIManager.Instance.UpdateItems(collectedItems, collectableItems);
        if (collectedItems >= collectableItems)
        {
            OnGameOverCallback?.Invoke();
        }
    }
}
