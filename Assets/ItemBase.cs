using UnityEngine;

public class ItemBase : MonoBehaviour
{
    [SerializeField]
    private float rotationSpeed;
    [SerializeField]
    private float floatingSpeed;
    private float angle = -1;
    private Vector3 startPos;

    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        transform.eulerAngles += new Vector3(0, rotationSpeed * Time.deltaTime, 0);

        angle += Time.deltaTime * floatingSpeed;
        if (angle >= 360)
        {
            angle = 0;
        }
        transform.position = new Vector3(startPos.x, startPos.y + Mathf.Sin(angle) * .05f, startPos.z);
    }
}
