using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField] float health;
    [SerializeField] float damage;
    [Space]
    [SerializeField] float speed;
    [SerializeField] Transform player;
    [Space]
    [SerializeField] float FOVdegree;
    [SerializeField] float radius;
    Rigidbody2D myRigid;
    Vector2 playerVector;
    bool isNear;
    SpriteRenderer spriteRenderer;
    float distanceDiff;
    float angle;




    private void Awake()
    {
        myRigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        isNear = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (player.position.x > transform.position.x)
            distanceDiff = player.position.x - transform.position.x;
        else
            distanceDiff = transform.position.x - player.position.x;
        playerVector = player.transform.position - transform.position;
        myRigid.AddForce(new Vector2(1, 0) * speed);
        
        
        
        if(spriteRenderer.flipX)
            angle = Vector2.SignedAngle(transform.right, playerVector);
        else
            angle = Vector2.SignedAngle(-transform.right, playerVector);
        
        if(distanceDiff  < radius)
        if (angle <= FOVdegree && angle >= -FOVdegree)
        {
            Debug.Log("In FOV");
            if (Physics2D.Raycast(transform.position , playerVector))
            {
                if (player.position.x > transform.position.x) { 
                    spriteRenderer.flipX = true;
                    if (speed < 0.01f)
                        speed = -speed;
                }
                else { 
                    spriteRenderer.flipX = false;
                    if(speed > 0.01f)
                        speed = -speed;
                }
                    isNear = true;
            }

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Wall")
        {
            spriteRenderer.flipX = !spriteRenderer.flipX;
            speed = -speed;
        }
        
    }

    void attack()
    {
        Debug.Log("Enemy Attacked");
    }
    public bool getIsNear()
    {
        return isNear; 
    }
    public void setSpeed(float speed)
    {
        speed = this.speed;
    }
    public void setIsNear(bool x)
    {
        x = this.isNear;
    }
}
