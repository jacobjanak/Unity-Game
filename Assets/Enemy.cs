using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // connections to the view
    public Animator animator;

    // keep track of enemy state
    int maxHealth = 100;
    int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        // start the hurt animation
        animator.SetTrigger("Hurt");

        // take damage
        currentHealth -= amount;
        Debug.Log("enemy health: " + currentHealth);
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // start the death animation
        animator.SetBool("IsDead", true);

        // remove the physics of the body
        GetComponent<Collider2D>().enabled = false;

        // disable this script
        this.enabled = false;
    }
}
