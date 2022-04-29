using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Take_Damage : MonoBehaviour
{
    public float maxHealth = 100;
    public float currentHealth;
    private Animator animator;
    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        animator.SetBool("beenHit", false);
    }

    public void TakeDamage(float damageTaken) 
    {
        if(currentHealth - damageTaken >= 0)
        {
            currentHealth -= damageTaken;
        } else {
            currentHealth = 0;
        }

        if(currentHealth <= 0) {
            animator.SetBool("hasDied", true);
        } else {
            animator.SetBool("beenHit", true);
            //animator.Play("Heavy Hit");
        }
    }
}
