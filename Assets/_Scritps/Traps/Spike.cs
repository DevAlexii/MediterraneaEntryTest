using System.Collections;
using UnityEngine;

public class Spike : MonoBehaviour
{
    private float coolDownActivation = 1f;
    private bool activated;

    [SerializeField]
    private float moveDistance = 1f;
    [SerializeField]
    private float moveDuration = 0.5f; 

    void Start()
    {
        activated = false;
        switch (GameManager.Instance.Difficulty)
        {
            case Difficulty.Easy:
                coolDownActivation = 1f;
                break;
            case Difficulty.Medium:
                coolDownActivation = .6f;
                break;
            case Difficulty.Hard:
                coolDownActivation = .2f;
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (activated)
            return;

        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {

            activated = true;

            StartCoroutine(ActivateTrap());
        }
    }

    IEnumerator ActivateTrap()
    {
        float timer = coolDownActivation;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }

        Vector3 originalPosition = transform.position;
        Vector3 targetPosition = transform.position + -transform.up * moveDistance;

        AudioManager.Instance.PlayClipSound(ClipType.Spike);
        float moveTimer = 0f;
        while (moveTimer < moveDuration)
        {
            transform.position = Vector3.Lerp(originalPosition, targetPosition, moveTimer / moveDuration);
            moveTimer += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;

        yield return new WaitForSeconds(0.5f); 

        moveTimer = 0f;
        while (moveTimer < moveDuration)
        {
            transform.position = Vector3.Lerp(targetPosition, originalPosition, moveTimer / moveDuration);
            moveTimer += Time.deltaTime;
            yield return null;
        }
        activated = false;
        transform.position = originalPosition;
    }

}
