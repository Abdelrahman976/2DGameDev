using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageScript : MonoBehaviour
{
    [SerializeField] float health;
    [SerializeField] Slider healthBar;
    [SerializeField] GameObject player;
    private Animator animator;
    bool flag = true;

    // Start is called before the first frame update
    private void Awake()
    {
        animator = player.GetComponent<Animator>();
    }

    void Start()
    {
        // Ensure healthBar's min and max values are set
        health = 1f;
        healthBar.minValue = 0;
        healthBar.maxValue = 1;
        healthBar.value = health;
        Debug.Log("Health at Start: " + health);
        Debug.Log("HealthBar value at Start: " + healthBar.value);
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.value = health;
        if (health <= 0)
        {
            Destroy(this.gameObject);
        }
        if(animator.GetBool("isIdle")==true)
            flag = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Attack") && animator.GetBool("isKicking") == true && flag)
        {
            health -= 0.1f;
            healthBar.value = health;
            flag = false;
        }
        if (collision.gameObject.CompareTag("Attack") && animator.GetBool("isPunching") == true && flag )
        {
            health -= 0.1f;
            healthBar.value = health;
            flag = false;
        }
    }

    public void makeDamage()
    {
        if ( flag)
        {
            health -= 0.1f;
            healthBar.value = health;
            flag = false;
        }
    }
}