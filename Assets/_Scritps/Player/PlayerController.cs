
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float speed  = 5f;
    private float moveSpeed;
    [SerializeField]
    public float rotationSpeed = 700f;

    private Rigidbody rb;

    bool controllerInputEnabled;

    GameObject pickableItem;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        controllerInputEnabled = true;
        moveSpeed = speed;
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
        controllerInputEnabled = true; 
        transform.position = Vector3.zero;
        moveSpeed = speed;
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
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Spike"))
        {
            OnHitEvent();
            StartCoroutine(SlowDown());
        }
    }

    IEnumerator SlowDown()
    {
        moveSpeed = .5f;
        yield return new WaitForSeconds(3);
        moveSpeed = speed;
    }

    private void OnHitEvent()
    {
        CameraShake.Shake(1f);
        AudioManager.Instance.PlayClipSound(ClipType.Hit);
        GameManager.Instance.WasteTime(5);
    }
}
