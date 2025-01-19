using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private static Camera mainCamera;
    private static Quaternion originalRotation;

    void Awake()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
            if (mainCamera != null)
            {
                originalRotation = mainCamera.transform.rotation;
            }
        }
    }

    public static void Shake(float force)
    {
        if (mainCamera != null)
        {
            GameManager.Instance.StartCoroutine(ShakeCoroutine(force));
        }
    }

    private static IEnumerator ShakeCoroutine(float force)
    {
        float shakeDuration = 0.1f;
        float shakeAmount = force; 
        float elapsedTime = 0f;

        Quaternion initialRotation = mainCamera.transform.rotation;

        while (elapsedTime < shakeDuration)
        {
            float shakeX = Random.Range(-shakeAmount, shakeAmount);
            float shakeY = Random.Range(-shakeAmount, shakeAmount);
            float shakeZ = Random.Range(-shakeAmount, shakeAmount);

            Quaternion shakeRotation = Quaternion.Euler(shakeX, shakeY, shakeZ);
            mainCamera.transform.rotation = initialRotation * shakeRotation;

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        mainCamera.transform.rotation = originalRotation;
    }
}
