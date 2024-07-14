using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skeleton : MonoBehaviour
{

    [SerializeField] float health;
    [SerializeField] float damage;
    [SerializeField] float speed;
    [Space]
    [SerializeField] Player player;

    Rigidbody2D myRigid;
    Animator myAnimator;   
    CapsuleCollider2D capsuleCollider;
    SpriteRenderer spriteRenderer;

    bool isDead = false;

    private void Awake()
    {
        myAnimator = GetComponent<Animator>();
        myRigid = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        speed = -speed;
        
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (player.transform.position.x > transform.position.x && speed < 0)
        {
            speed *= -1; 
            spriteRenderer.flipX = true;
        }
        else if(player.transform.position.x < transform.position.x && speed > 0)
        {
            speed *= -1;
            spriteRenderer.flipX = false;
        }

        myRigid.AddForce(Vector2.right * speed);

        if (health <= 0) {
            speed = 0;
            isDead = true;
        }

        myAnimator.SetBool("isDead" , isDead);
    }

    public float getDamage()
    {

        return damage; 
    }

    void stopSpeed()
    {
        myRigid.bodyType = RigidbodyType2D.Static;
    }

    void startSpeed()
    {
        myRigid.bodyType = RigidbodyType2D.Dynamic;
    }
    void death()
    {
        Destroy(gameObject);
    }
}
