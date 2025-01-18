using UnityEngine;

public class Camera : MonoBehaviour
{
    [SerializeField]
    private Transform Target; 
    [SerializeField]
    private Vector3 cameraOffset = new Vector3(0, 2, -1); 
    [SerializeField]
    private float smoothTime = 0.2f; 

    private Vector3 velocity = Vector3.zero; 

    void LateUpdate()
    {
        if (Target)
        {
           
            Vector3 targetPosition = Target.position + cameraOffset;

            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        }
    }
}
