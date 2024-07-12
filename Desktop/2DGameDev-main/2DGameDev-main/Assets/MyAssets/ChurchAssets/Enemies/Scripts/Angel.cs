using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Angel : MonoBehaviour
{
    [SerializeField] float health;
    [SerializeField] float speed;
    [SerializeField] float damage;

    CapsuleCollider2D capsuleCollider;
    Rigidbody2D myRigid;
    Animator myAnimator;

    bool dead = false;

    private void Awake()
    {
        myAnimator = GetComponent<Animator>();
        myRigid = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
    }

    void Update()
    {
        if (!dead)
        {
            // Move the Angel vertically using velocity
            myRigid.velocity = new Vector2(myRigid.velocity.x, speed);
        }
        myAnimator.SetFloat("health", health);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Barrier"))
        {
            // Reverse the vertical movement direction
            speed *= -1;
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