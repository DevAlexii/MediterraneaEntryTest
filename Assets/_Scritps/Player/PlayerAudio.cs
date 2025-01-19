using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    AudioSource audioSource;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void PlayFootStep()
    {
        audioSource.pitch = Random.Range(0.8f, 1.2f); 
        audioSource.Play();
    }
}
