using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghoul : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float health;
    [SerializeField] float damage;

    bool dead = false;

    Rigidbody2D myRigid;
    SpriteRenderer spriteRenderer;
    Animator animator;
    CapsuleCollider2D capsuleCollider;

    private void Awake()
    {
        myRigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
    }

    void Update()
    {
        if (!dead)
        {
            // Move the Ghoul horizontally
            myRigid.velocity = new Vector2(speed, myRigid.velocity.y);
        }
        animator.SetFloat("health", health);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            // Flip the sprite horizontally
            spriteRenderer.flipX = !spriteRenderer.flipX;
            // Reverse the speed to change direction
            speed = -speed;
        }
    }

    void death()
    {
        dead = true;
        myRigid.bodyType = RigidbodyType2D.Static;
        capsuleCollider.enabled = false;
    }

    void destroyEnemy()
    {
        Destroy(this.gameObject);
    }

    public float getDamage()
    {
        return damage;
    }
}