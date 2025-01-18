using UnityEngine;

public class PlayerAnimController : MonoBehaviour
{
    Rigidbody rb;
    Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        animator.SetBool("isMoving", rb.velocity.magnitude > .01f);
    }
}
