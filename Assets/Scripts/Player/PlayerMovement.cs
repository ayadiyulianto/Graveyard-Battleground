using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Transform groundCheck;
    public float groundDistance = .3f;
    public LayerMask groundMask;
    
    public float moveSpeed = 1f;
    public float jumpHeight = 2f;
    public Animator animator;
    
    Vector3 velocity = Vector3.zero;
    bool isGrounded = false;
    float gravity = -9.81f;
    CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if(isGrounded)
        {
            animator.SetBool("IsGrounded", true);
            if(velocity.y < 0) velocity.y = -1f;
        }
        else
        {
            animator.SetBool("IsGrounded", false);
        }

        // Move
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * y;
        controller.Move(move * moveSpeed * Time.deltaTime);
        animator.SetFloat("PosX", x);
        animator.SetFloat("PosY", y);

        // Jump
        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }
}
