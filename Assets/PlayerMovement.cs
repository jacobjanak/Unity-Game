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
    public bool inputWasd = true;
    public bool inputArrows = false;

    // keep track of player state
    float horizontalMove = 0f;
    bool jump = false;
    bool backpedal = false;

    // Update runs once per frame, FixedUpdate runs once per x frames
    // capture user input in Update and respond to user input in FixedUpdate
    void Update()
    {
        // which direction player is facing
        if (inputWasd)
        {
            // set direction of movement
            if (Input.GetKey(KeyCode.A))
            {
                horizontalMove = -1f;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                horizontalMove = 1f;
            }
            else
            {
                horizontalMove = 0;
            }
        }
        else if (inputArrows)
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                horizontalMove = -1f;
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                horizontalMove = 1f;
            }
            else
            {
                horizontalMove = 0;
            }
        }
        // horizontalMove = Input.GetAxisRaw("Horizontal");

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
        if (inputWasd)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                jump = true;
            }
        }
        if (inputArrows)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                jump = true;
            }
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
