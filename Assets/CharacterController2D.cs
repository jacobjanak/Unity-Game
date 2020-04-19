using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{
	public LayerMask m_WhatIsGround;						 // A mask determining what is ground to the character
	public Transform m_GroundCheck;							 // A position marking where to check if the player is grounded.

	public bool m_AirControl = false;						 // Whether or not a player can steer while jumping;
	[Range(0, .3f)] public float m_MovementSmoothing = .05f; // How much to smooth out the movement
	public float m_JumpForce = 400f;						 // Amount of force added when the player jumps.

	const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
	private bool m_Grounded = false; // Whether or not the player is grounded.
	const float k_CeilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
	private Rigidbody2D m_Rigidbody2D;
	private bool m_FacingRight = true;  // For determining which way the player is currently facing.
	private Vector3 m_Velocity = Vector3.zero;

	[Header("Events")] [Space] public UnityEvent OnLandEvent;

	private void Awake()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();

		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();
	}

	private void FixedUpdate()
	{
		bool wasGrounded = m_Grounded;
		m_Grounded = false;

		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.
		Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				m_Grounded = true;
				if (!wasGrounded)
					OnLandEvent.Invoke();
			}
		}
	}

	public void Move(float move, bool jump, bool backpedal)
	{
		//only control the player if grounded or airControl is turned on
		if (m_Grounded || m_AirControl)
		{
			// Move the character by finding the target velocity
			Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);

			// And then smoothing it out and applying it to the character
			m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

			// If the input is moving the player right and the player is facing left...
            if (move > 0 && !m_FacingRight && !backpedal)
            {
                Flip();
            }
            else if (move < 0 && m_FacingRight && !backpedal)
            {
                Flip();
            }
            else if (move > 0 && m_FacingRight && backpedal)
            {
                Flip();
            }
            else if (move < 0 && !m_FacingRight && backpedal)
            {
                Flip();
            }
		}

		// If the player should jump...
		if (m_Grounded && jump)
		{
			// Add a vertical force to the player.
			m_Grounded = false;
			m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
		}
	}

	public void Flip()
	{
		// Switch the way the player is labelled as facing.
		m_FacingRight = !m_FacingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

    public bool IsGrounded() {
        return m_Grounded;
    }

    // helpful for editting the sprite in Unity
    void OnDrawGizmosSelected()
    {
        if (m_GroundCheck == null) return;
        Gizmos.DrawWireSphere(m_GroundCheck.position, k_GroundedRadius);
    }
}
