using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    // connections to the view
    public CharacterController2D controller;
    public Animator animator;
    public Transform swordHitBox;
    public Transform flurryHitBox;
    public Transform executeHitBox;
    public LayerMask enemyLayers;

    // customizations
    public float attackRate = 2f; // per second
    public float swordRange = 0.5f;
    public float swordDamage = 5f;
    // public int berserkCooldown = 15;
    // public float maxBerserkTime = 5f;
    public float flurryRange = 0.8f;
    public float flurryDamage = 1f;
    public float executeRange = 0.2f;
    public float executeDamage = 10f;

    // keep track of state
    float nextAttackTime = 0f;
    float nextLeftSwordTime = 0f;
    float nextRightSwordTime = 0f;
    float hitsInRow = 0f;
    // float nextBerserkTime = 0f;
    // float berserkTimeLeft = 0f;

    void Update()
    {
        // manage berserk time
        // if (berserkTimeLeft > 0)
        // {
        //     berserkTimeLeft -= Time.deltaTime;
        //     if (berserkTimeLeft <= 0 )
        //     {
                
        //     }
        // }

        // can only attack after a cooldown period

        // attack key 1: left sword
        if (Time.time > nextLeftSwordTime)
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                // attack cannot be used in the air
                if (controller.IsGrounded())
                {
                    Sword();
                    nextLeftSwordTime = Time.time + 1f / attackRate; // NOTE: make this a function
                }
            }
        }

        // attack key 2: right sword
        if (Time.time > nextRightSwordTime)
        {
            if (Input.GetKeyDown(KeyCode.O))
            {
                // attack cannot be used in the air
                if (controller.IsGrounded())
                {
                    Sword();
                    nextRightSwordTime = Time.time + 1f / attackRate; // NOTE: make this a function
                }
            }
        }

        // attack key 3 and 4        
        if (Time.time > nextLeftSwordTime && Time.time > nextRightSwordTime)
        {
            // attack key 3: flurry
            if (Input.GetKeyDown(KeyCode.K))
            {
                Flurry();
                nextAttackTime = Time.time + 1f / attackRate; // NOTE: make this a function
                nextLeftSwordTime = nextAttackTime;
                nextRightSwordTime = nextAttackTime;
            }

            else if (Input.GetKeyDown(KeyCode.L))
            {
                Execute();
                nextAttackTime = Time.time + 1f / attackRate; // NOTE: make this a function
                nextLeftSwordTime = nextAttackTime;
                nextRightSwordTime = nextAttackTime;
            }
        }

        // if (Time.time > nextAttackTime)
        // {
        //     // attack key 1
        //     if (Input.GetKeyDown(KeyCode.J))
        //     {
        //         // attack cannot be used in the air
        //         if (controller.IsGrounded())
        //         {
        //             LeftSword();
        //             nextAttackTime = Time.time + 1f / attackRate; // NOTE: make this a function
        //         }
        //     }

        //     else if (Input.GetKeyDown(KeyCode.L))
        //     {
        //         Flurry();
        //         nextAttackTime = Time.time + 1f / attackRate; // NOTE: make this a function
        //     }
        // }

        // attack key 1
        // if (Time.time > nextBerserkTime)
        // {
        //     if (Input.GetKeyDown(KeyCode.K))
        //     {
        //         Berserk();
        //         nextBerserkTime = Time.time + berserkCooldown;
        //     }
        // }
    }

    void Sword()
    {
        // start attack animation
        animator.SetTrigger("Attack");

        // detect enemies
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(swordHitBox.position, swordRange, enemyLayers);

        // calculate current damage
        float damage = CalculateDamage(swordDamage);

        // apply damage to enemies
        foreach(Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(damage);
        }

        // increment hits in a row
        if (hitEnemies.Length > 0) hitsInRow++;
        else hitsInRow = 0f;
    }

    // void Berserk() {
    //     berserkTimeLeft = maxBerserkTime;
    // }

    void Flurry()
    {
        // start attack animation
        // animator.SetTrigger("Flurry");

        // detect enemies
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(flurryHitBox.position, flurryRange, enemyLayers);

        // calculate current damage
        float damage = CalculateDamage(flurryDamage);
        // if (berserkTimeLeft > 0)
        // {
        //     damage *= 2;
        // }

        // apply damage to enemies
        foreach(Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(damage);
        }

        // increment hits in a row
        if (hitEnemies.Length > 0) hitsInRow++;
        else hitsInRow = 0f;
    }

    void Execute()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(executeHitBox.position, executeRange, enemyLayers);

        // calculate current damage
        float damage = CalculateDamage(executeDamage);

        // apply damage to enemies
        foreach(Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(damage);
        }

        // increment hits in a row
        if (hitEnemies.Length > 0) hitsInRow++;
        else hitsInRow = 0f;
    }

    float CalculateDamage(float damage)
    {
        return (float) System.Math.Floor(damage * (10f + hitsInRow * hitsInRow) / 10f);
    }

    // method to allow easier editting in Unity
    void OnDrawGizmosSelected()
    {
        if (swordHitBox == null) return;
        Gizmos.DrawWireSphere(swordHitBox.position, swordRange);

        if (flurryHitBox == null) return;
        Gizmos.DrawWireSphere(flurryHitBox.position, flurryRange);

        if (executeHitBox == null) return;
        Gizmos.DrawWireSphere(executeHitBox.position, executeRange);
    }
}
