using UnityEngine;

public enum Difficulty
{
    Easy,
    Medium,
    Hard
}

public class GameManager : MonoBehaviour
{
    private static Difficulty difficulty;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public static void ChangeDifficulty(int newDifficulty)
    {
        difficulty = (Difficulty)newDifficulty;
    }
}
