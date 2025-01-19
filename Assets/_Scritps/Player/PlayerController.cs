
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 700f;

    private Rigidbody rb;

    bool controllerInputEnabled;

    GameObject pickableItem;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        controllerInputEnabled = true;
    }

    private void OnEnable()
    {
        GameManager.OnGameOverCallback += OnGameOver;
        GameManager.OnRestartGameCallback += OnRestart;
    }

    private void OnDisable()
    {
        GameManager.OnGameOverCallback -= OnGameOver;
        GameManager.OnRestartGameCallback -= OnRestart;
    }

    private void OnGameOver()
    {
        controllerInputEnabled = false;
    }
    private void OnRestart()
    {
        controllerInputEnabled = true; transform.position = Vector3.zero;
    }

    private void Update()
    {
        if (controllerInputEnabled)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (pickableItem)
                {
                    pickableItem.GetComponent<ICollectable>().OnCollected();
                    pickableItem = null;
                }
            }
        }
    }

    void FixedUpdate()
    {
        if (controllerInputEnabled)
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;

            rb.velocity = direction * moveSpeed;

            if (direction.magnitude >= 0.1f)
            {
                Quaternion toRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.fixedDeltaTime);
            }
            else
            {
                rb.velocity = Vector3.zero;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Item"))
        {
            GameUIManager.Instance.ShowInteracitonPopUP(other.transform.position);
            pickableItem = other.gameObject;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Item"))
        {
            GameUIManager.Instance.DeactiveInteractionPopUP();
            pickableItem = null;
        }
    }
}
