using UnityEngine;

public enum ClipType
{
    Button,
    Collected
}

public enum BackgroundType
{
    Menu,
    Game
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Clips Audio")]
    [SerializeField]
    private AudioClip buttonClip;
    [SerializeField]
    private AudioClip menuMusicClip;
    [SerializeField]
    private AudioClip collectedClip;
    [SerializeField]
    private AudioClip gameMusicClip;

    [Header("Audio Sources")]
    [SerializeField]
    private AudioSource UISource;
    [SerializeField]
    private AudioSource backgroundSource;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void PlayClipSound(ClipType clipType)
    {
        switch (clipType)
        {
            case ClipType.Button:
                UISource.PlayOneShot(buttonClip);
                break;
            case ClipType.Collected:
                UISource.PlayOneShot(collectedClip);
                break;
        }
    }

    public void PlayBackground(BackgroundType bgType)
    {
        switch (bgType)
        {
            case BackgroundType.Menu:
                backgroundSource.clip = menuMusicClip;
                backgroundSource.loop = true;
                backgroundSource.Play();
                break;
            case BackgroundType.Game:
                backgroundSource.clip = gameMusicClip;
                backgroundSource.loop = true;
                backgroundSource.Play();
                break;
        }
    }
}
