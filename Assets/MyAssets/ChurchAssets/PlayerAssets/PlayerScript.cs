using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Serialized Fields
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float speed = 5f;
    [SerializeField] private GameObject Foot;

    // Private Fields
    private PlayerControls controls;
    private float moveDirection;
    private Animator animator;
    private Rigidbody2D rb;
    private bool isJumping;
    private bool isIdle;

    void Awake()
    {
        controls = new PlayerControls();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        // Movement actions
        controls.Movement.Movement.performed += ctx => moveDirection = ctx.ReadValue<float>();
        controls.Movement.Movement.canceled += ctx => moveDirection = 0f;
        controls.Movement.Jump.performed += OnJump;

        // Attack actions
        controls.Attack.Kick.performed += ctx => OnKick();
        controls.Attack.Punch.performed += ctx => OnPunch();
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (isGrounded())
        {
            isJumping = true;
            isIdle = false;
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        }
    }

    private void OnKick()
    {
        Debug.Log("Kicking");
        // Add your kick logic here
    }

    private void OnPunch()
    {
        Debug.Log("Punching");
        // Add your punch logic here
    }

    void OnEnable()
    {
        controls.Enable();
    }

    void OnDisable()
    {
        controls.Disable();
    }

    void Update()
    {
        HandleMovement();
        HandleAnimations();
    }

    private void HandleMovement()
    {
        Vector2 move = new Vector2(moveDirection * speed, rb.velocity.y);
        rb.velocity = move;
    }

    private void HandleAnimations()
    {
        if (isGrounded() && rb.velocity.y <= 0)
        {
            isJumping = false;
            isIdle = true;
            animator.Play("Idle");
        }
        animator.SetBool("isIdle", isIdle);
        animator.SetBool("isJumping", isJumping);
    
        Debug.Log("isIdle-------------------------------" + isIdle);
    }

    bool isGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(Foot.transform.position, Vector2.down, 0.5f, LayerMask.GetMask("Ground"));
        Debug.DrawRay(Foot.transform.position, Vector2.down, Color.blue, 0.5f);
        return hit.collider != null;
    }
}
