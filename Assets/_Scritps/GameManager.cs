using UnityEngine;

public enum Difficulty
{
    Easy,
    Medium,
    Hard
}

public class GameManager : MonoBehaviour
{
    private Difficulty difficulty;

    void Start()
    {
        DontDestroyOnLoad(gameObject);

        difficulty = (Difficulty)SettingSaver.currentSetting.difficulty;
    }
}
