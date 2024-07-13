using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float speed = 5f;
    [SerializeField] private GameObject Foot;

    private PlayerControls controls;
    private float moveDirection;
    private Animator animator;
    private Rigidbody2D rb;
    private Transform playerTransform;
    private bool JumpActive;
    private BoxCollider2D playerCollider;
    private float playerHeight;
    private float yOffset;
    private Vector2 originalOffset;
    private bool isCrouching;
    [SerializeField] private Slider healthbar;
 
    public float health = 100;

    void Awake()
    {
        controls = new PlayerControls();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        playerTransform = GetComponent<Transform>();
        playerCollider = GetComponent<BoxCollider2D>();
        originalOffset = playerCollider.offset;
        playerHeight= playerCollider.size.y;
        yOffset =(float) (playerHeight - playerHeight*0.2) / 2f;
        healthbar.value = health/100;

        // Movement actions
        controls.Movement.Movement.performed += ctx => moveDirection = ctx.ReadValue<float>();
        controls.Movement.Movement.canceled += ctx => moveDirection = 0f;
        controls.Movement.Jump.performed += OnJump;

        // Attack actions
        controls.Attack.Kick.performed += ctx => OnKick();
        controls.Attack.Kick.canceled += ctx => EndAnimation("isKicking");
        controls.Attack.Punch.performed += ctx => OnPunch();
        controls.Attack.Punch.canceled += ctx => EndAnimation("isPunching");
        
        //Crouch actions
        controls.Movement.Crouch.performed += ctx => OnCrouch();
        controls.Movement.Crouch.canceled += ctx => EndAnimation("isCrouching");
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (isGrounded())
        {
            animator.SetBool("isIdle", false);
            animator.SetBool("isJumping", true);
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            JumpActive = true;
        }
        
    }
    private void OnCrouch()
    {
        // Example of reducing the player's height and setting crouch animation
        animator.SetBool("isCrouching", true);
        isCrouching = true;
        playerCollider.size = new Vector2(playerCollider.size.x, playerHeight*0.5f);
        playerCollider.offset = new Vector2(playerCollider.offset.x, -yOffset);
        
    }

    private void OnKick()
    {
        animator.SetBool("isKicking", true);
        //animator.Play("Kicking");
    }

    private void OnPunch()
    {
        animator.SetBool("isIdle",false);
        animator.SetBool("isPunching", true);
    }

    public void EndAnimation(string name)
    {
        animator.SetBool(name, false);
       animator.Play("Idle");
       if(isCrouching)
       {
           playerCollider.size = new Vector2(playerCollider.size.x, playerHeight);
           playerCollider.offset = originalOffset;
           isCrouching = false;
       }
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
        if (!isCrouching && !animator.GetBool("takeDamage"))
        {
            HandleMovement();
            HandleJumpAnimation();
        }
        healthbar.value = health/100f;

    }

    private void HandleMovement()
    {
        Vector2 move = new Vector2(moveDirection * speed, rb.velocity.y);
        rb.velocity = move;

        // Check direction and flip the player accordingly
        if (moveDirection > 0)
        {
            playerTransform.localScale = new Vector3(Mathf.Abs(playerTransform.localScale.x), playerTransform.localScale.y, playerTransform.localScale.z);
        }
        else if (moveDirection < 0)
        {
            playerTransform.localScale = new Vector3(-Mathf.Abs(playerTransform.localScale.x), playerTransform.localScale.y, playerTransform.localScale.z);
        }

        // Walking animation
        bool isWalking = moveDirection != 0;
        animator.SetBool("isWalking", isWalking);
        if (!isWalking && isGrounded())
        {
            animator.SetBool("isIdle", true);
        }
        else if(isWalking && !isGrounded())
        {
            animator.SetBool("isWalking", false);
        }
        else
        {
            animator.SetBool("isIdle", false);
        }
    }

    private void HandleJumpAnimation()
    {
        if (isGrounded() && rb.velocity.y <= 0 && JumpActive)
        {
           animator.SetBool("isIdle", true);
           animator.SetBool("isJumping", false);
           JumpActive = false;
        }
        
    }

    bool isGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(Foot.transform.position, Vector2.down, 0.6f, LayerMask.GetMask("Ground"));
        Debug.DrawRay(Foot.transform.position, Vector2.down, Color.blue, 0.5f);
        return hit.collider != null;
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("FireBall") )
        {
            animator.SetBool("takeDamage", true);
            health -= 10;
            
            StartCoroutine(Sleep());
        }
       
    }
    void OnCollisionEnter2D(Collision2D collision)
    {   
            
        if ( collision.gameObject.CompareTag("Ghoul") || collision.gameObject.CompareTag("Angel"))
        {
            Debug.Log("Player collided with enemy");
            animator.SetBool("takeDamage", true);
            health -= 10;
            StartCoroutine(Sleep());
        }
        
    }
    IEnumerator Sleep()
    {
        yield return new WaitForSeconds(1f);
        animator.SetBool("takeDamage", false);
    }
}

