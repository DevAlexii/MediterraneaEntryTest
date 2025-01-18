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


    private Difficulty difficulty;

    private void Start()
    {
        OnRestartGameCallback += ResetData;

        ResetData();
    }

    private void ResetData()
    {
        currentCountDown = gameCountDown;
        collectedItems = 6;
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
}
