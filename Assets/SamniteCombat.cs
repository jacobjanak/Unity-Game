using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamniteCombat : MonoBehaviour
{
    // connections to the view
    public CharacterController2D controller;
    public Animator animator;
    public Transform swordHitBox;
    // public Transform flurryHitBox;
    // public Transform executeHitBox;
    public LayerMask enemyLayers;

    // customizations
    public float attackRate = 2f; // per second
    public float swordRange = 0.5f;
    public float swordDamage = 3f;
    // // public int berserkCooldown = 15;
    // // public float maxBerserkTime = 5f;
    // public float flurryRange = 0.8f;
    // public float flurryDamage = 1f;
    // public float executeRange = 0.2f;
    // public float executeDamage = 10f;

    // // keep track of state
    float nextAttackTime = 0f;
    // float nextLeftSwordTime = 0f;

    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextAttackTime)
        {
            // attack key 1: Sword
            if (Input.GetKeyDown(KeyCode.R))
            {
                // attack cannot be used in the air
                if (controller.IsGrounded())
                {
                    Sword();
                    nextAttackTime = Time.time + 1f / attackRate; // NOTE: make this a function
                }
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                // attack cannot be used in the air
                if (controller.IsGrounded())
                {
                    Charge();
                    nextAttackTime = Time.time + 1f / attackRate; // NOTE: make this a function
                }
            }
        }
    }

    void Sword()
    {
        // start attack animation
        animator.SetTrigger("Attack");

        // detect enemies
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(swordHitBox.position, swordRange, enemyLayers);

        // apply damage to enemies
        foreach(Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(swordDamage);
        }
    }

    void Charge()
    {
        // get direction as float
        // Vector3 temp = new Vector3(7.0f * direction, 0, 0);
        // myGameObject.transform.position += temp;
    }

    // method to allow easier editting in Unity
    void OnDrawGizmosSelected()
    {
        if (swordHitBox == null) return;
        Gizmos.DrawWireSphere(swordHitBox.position, swordRange);
    }
}
