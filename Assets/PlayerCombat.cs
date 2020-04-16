using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    // connections to the view
    public Animator animator;
    public Transform attackPoint;
    public LayerMask enemyLayers;

    // customizations
    public float attackRange = 0.5f;
    public int attackDamage = 40;
    public float attackRate = 2f; // per second

    // keep track of state
    float nextAttackTime = 0f;

    void Update()
    {
        // can only attack after a cooldown period
        if (Time.time > nextAttackTime)
        {
            // attack key 1
            if (Input.GetKeyDown(KeyCode.J))
            {
                Attack();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
    }

    void Attack() {

        // start attack animation
        animator.SetTrigger("Attack");

        // detect enemies
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        // apply damage to enemies
        foreach(Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
        }
    }

    // method to allow easier editting in Unity
    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
