using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Wizard : MonoBehaviour
{

    [SerializeField] float Health;
    [SerializeField] float damage;

    [Space]
    [SerializeField] float FOVdegree;
    [SerializeField] float radius;
    [SerializeField] Player player;
    [Space]
    [Space]
    [SerializeField] GameObject Fireball;

    Rigidbody2D myRigid;
    Vector2 playerVector;
    SpriteRenderer spriteRenderer;
    float distanceDiff;
    float angle;
    bool isNear;
    Animator animator;
    bool isAttacking;

    private void Awake()
    {
        isAttacking = false;
        isNear = false;
        myRigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().flipX = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.transform.position.x > transform.position.x)
            distanceDiff = player.transform.position.x - transform.position.x;
        else
            distanceDiff = transform.position.x - player.transform.position.x;


        playerVector = player.transform.position - transform.position;



        if (spriteRenderer.flipX)
            angle = Vector2.SignedAngle(transform.right, playerVector);
        else
            angle = Vector2.SignedAngle(-transform.right, playerVector);

        if (distanceDiff < radius)
            if (angle <= FOVdegree && angle >= -FOVdegree)
            {
                Debug.Log("In FOV");
                if (Physics2D.Raycast(transform.position, playerVector))
                {
                    if (player.transform.position.x > transform.position.x)
                    {
                        spriteRenderer.flipX = true;
                    }
                    else
                    {
                        spriteRenderer.flipX = false;
                    }
                    isNear = true;
                    attack();
                    
                }

            }
        if (isPlayerDead())
            animator.ResetTrigger("Return");
    }

    void attack()
    {

        animator.SetTrigger("Attack");
        Debug.Log("Attacking");
        
        


    }
    

    bool isPlayerDead()
    {
        if (player.health <= 0)
            return true;
        return false;
    }

    void spawnFireBall()
    {
        GameObject obj = Instantiate(Fireball, transform.position, Quaternion.identity);
        if (spriteRenderer.flipX)
            obj.transform.position = new Vector2(obj.transform.position.x + 1.05f , obj.transform.position.y);  
        else
            obj.transform.position = new Vector2(obj.transform.position.x - 1.05f , obj.transform.position.y);  

            Rigidbody2D rigidbody = obj.GetComponent<Rigidbody2D>();
        if (spriteRenderer.flipX)
            rigidbody.AddForce(transform.right * 5, ForceMode2D.Impulse);
        else
            rigidbody.AddForce(-transform.right * 5, ForceMode2D.Impulse);


        Destroy(obj, 5);
    }
}
