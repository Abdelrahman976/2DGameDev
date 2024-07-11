using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!dead)
        {
            myRigid.AddForce(new Vector2(1, 0) * speed);
        }
        animator.SetFloat("health" , health);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            spriteRenderer.flipX = !spriteRenderer.flipX;
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
