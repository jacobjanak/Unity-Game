using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // connections to the view
    public CharacterController2D controller;
    public Animator animator;

    // customizations
    public float runSpeed = 15f;

    // keep track of player state
    float horizontalMove = 0f;
    bool jump = false;
    bool backpedal = false;

    // Update runs once per frame, FixedUpdate runs once per x frames
    // capture user input in Update and respond to user input in FixedUpdate
    void Update()
    {
        // which direction player is facing
        horizontalMove = Input.GetAxisRaw("Horizontal");

        // shift key flips direction
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            backpedal = true;

            // character controller handles flipping during movement
            if (horizontalMove == 0) controller.Flip();
        }

        // shift key
        else if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift))
        {
            backpedal = false;

            // character controller handles flipping during movement
            if (horizontalMove == 0)
            {
                controller.Flip();
            }
        }

        // jumping
        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
        }
    }

    void FixedUpdate()
    {
        // player movement
        float movement = horizontalMove * runSpeed * Time.fixedDeltaTime;
        controller.Move(movement, jump, backpedal);

        // running animation
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        // start jumping animation
        if (jump) {
            animator.SetBool("IsJumping", true);
            jump = false;
        }
    }

    // on landing event
    public void OnLanding() {
        animator.SetBool("IsJumping", false);
    }
}
