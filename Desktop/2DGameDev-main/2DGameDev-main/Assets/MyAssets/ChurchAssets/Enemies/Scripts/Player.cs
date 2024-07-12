using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D rigid;
    public float health;
    [SerializeField] Ghoul ghoul;
    [SerializeField] Angel angel;

    Animator AngelAnimator;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        AngelAnimator = angel.GetComponent<Animator>();
        health = 100;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rigid.AddForce(new Vector2(1, 0) * 0.7f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("AngelAttack"))
        {
            AngelAnimator.SetTrigger("attack");
            health -= angel.getDamage();
            rigid.AddForce(new Vector2(-1 ,0)* 10 , ForceMode2D.Impulse);

        }
        if (collision.gameObject.CompareTag("GhoulAttack"))
        {
            health -= ghoul.getDamage();
            rigid.AddForce(new Vector2(-1, 0) * 10, ForceMode2D.Impulse);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("AngelAttack"))
        {
            AngelAnimator.ResetTrigger("attack");
        }
    }
}
