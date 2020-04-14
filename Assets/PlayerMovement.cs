using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public CharacterController2D controller;
    public Animator animator;

    public float runSpeed = 40f;

    float horizontalMove = 0f;

    bool jump = false;
    bool backpedal = false;

    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal");

        // animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        // Shift key flips direction
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            backpedal = true;
            controller.Flip();
        }

        else if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift))
        {
            backpedal = false;

            if (horizontalMove == 0)
            {
                controller.Flip();
            }
        }

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
            // animator.SetBool("IsJumping", true);
        }
    }

    void FixedUpdate()
    {
        float movement = horizontalMove * runSpeed * Time.fixedDeltaTime;

        controller.Move(movement, false, jump, backpedal);

        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        if (jump) {
            animator.SetBool("IsJumping", true);
            jump = false;
        }


        // reset
        
    }

    public void OnLanding() {
        animator.SetBool("IsJumping", false);
    }
}
