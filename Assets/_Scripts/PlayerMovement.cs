using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D RB { get; private set; }
	Vector2 m_MoveInput;
    TrailRenderer m_TrailRenderer;
    Animator m_Animator;
    SpriteRenderer m_SpriteRenderer;

    public float runSpeed = 10f;
    public float velPower = 0.96f;
    public float jumpForce = 13f;
    //afloat time is the time range where someone can jump even if they are no longer touching the ground. Makes the game feel 
    public float stayAfloatTime = 0.1f;
    public float JumpBuffer = 0.15f;
    bool m_IsJumping;
    public float JumpFallMultiplier = 0.1f;
    public float playerGravity;
    public bool facingRight = true;

    public Vector2 groundCheckSize = new Vector2(0.5f, 0.2f);
    [SerializeField] Transform m_GroundCheckPoint;
    [SerializeField] LayerMask m_GroundLayer;

    [SerializeField] float m_DashingVelocity = 33f;
    [SerializeField] float m_DashLength = 0.2f;
    [SerializeField] Vector2 m_DashingDirection;
    bool m_IsDashing;
    float m_DashBuffer = 2f;
    float m_LastDashTime;

    float m_LastOnGroundTime; //Check when the player was on the ground so they can still jump if they JUST left a platform
    float m_LastJumpTime; //Check when the player last jumped so that they cannot jump again until a set amount of time

    private void Awake()
	{
		RB = GetComponent<Rigidbody2D>();
        playerGravity = RB.gravityScale;
        m_TrailRenderer = GetComponent<TrailRenderer>();
        m_Animator = GetComponent<Animator>();
        m_SpriteRenderer = GetComponent < SpriteRenderer >();
	}


    private void Update()
    {
        //Set up timers for all of my time-based movement checks
        m_LastOnGroundTime -= Time.deltaTime;
        m_LastJumpTime -= Time.deltaTime;
        m_LastDashTime -= Time.deltaTime;
        m_MoveInput.x = Input.GetAxisRaw("Horizontal");
        m_MoveInput.y = Input.GetAxisRaw("Vertical");

        if((m_MoveInput.x > 0 && !facingRight) || (m_MoveInput.x < 0 && facingRight))
        {
            FlipCharacter();
        }

        if(IsGrounded() && m_LastJumpTime < 0)
        {
            m_Animator.SetBool("isFalling", false);
            m_IsJumping = false;
            m_Animator.SetBool("isJumping", m_IsJumping);
        } else if(RB.velocity.y < 0)
        {
            m_Animator.SetBool("isFalling", true);
        }

        if(Input.GetKeyDown(KeyCode.LeftShift) && m_LastDashTime < 0)
        {
            Dash();
        }
        if (m_IsDashing)
        {
            RB.velocity = m_DashingDirection.normalized * m_DashingVelocity;
        }

        //Is player on the ground this frame? We can reset the afloat timer. Otherwise it will remain counting down.
        if (Physics2D.OverlapBox(m_GroundCheckPoint.position, groundCheckSize, 0, m_GroundLayer))
        {
            m_LastOnGroundTime = stayAfloatTime;
        }
        
        if(RB.velocity.y < 0)
        {
            RB.gravityScale = playerGravity * 3f;
        } else
        {
            RB.gravityScale = playerGravity;
        }


        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded() && m_LastJumpTime < 0)
        {
            m_IsJumping = true;
            m_Animator.SetBool("isJumping", m_IsJumping);
            Jump();
        }

        if (!m_IsJumping && Mathf.Abs(RB.velocity.x) > 1f)
        {
            m_Animator.SetBool("isWalking", true);
        } else if(!m_IsJumping && Mathf.Abs(RB.velocity.x) < 1f)
        {
            m_Animator.SetBool("isWalking", false);
        }

            //This code allows the player to fall faster after releasing the space key, essentially letting them cancel their jump in the middle.
        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (RB.velocity.y > 0 && m_IsJumping)
            {
                RB.AddForce(Vector2.down * RB.velocity.y * (1 - JumpFallMultiplier), ForceMode2D.Impulse);
            }
        }

        
    }

    private void FixedUpdate()
    {
        Run();
    }

    pivate void Dash()
    {
        m_IsDashing = true;
        m_LastDashTime = m_DashBuffer;
        m_TrailRenderer.emitting = true;
        m_DashingDirection = new Vector2(m_MoveInput.x, m_MoveInput.y);
        if (m_DashingDirection == Vector2.zero)
        {
            if (facingRight)
            {
                m_DashingDirection = new Vector2(1, 0);
            }
            else
            {
                m_DashingDirection = new Vector2(-1, 0);
            }
        }
        StartCoroutine(StopDashing());
    }

    private void Run()
    {
        float maxSpeed = runSpeed * m_MoveInput.x;
        float speedDifference = maxSpeed - RB.velocity.x;

        //We want faster acceleration when slowing down than when speeding up
        float acceleration;
        if(speedDifference > 0.01f)
        {
            acceleration = 13f;
        }
        else
        {
            acceleration = 16f;
        }

        float movement = Mathf.Abs(speedDifference) * acceleration * Mathf.Sign(speedDifference);

        RB.AddForce(movement * Vector2.right, ForceMode2D.Force);
    }

    private void Jump()
    {
        //Reset m_LastOnGroundTime to 0 so that, on the next frame, the timer will again be 0 unless they touch the ground, and therefore they won't be able to use the afloat timer to make a jump.
        m_LastOnGroundTime = 0;
        //Reset m_LastJumpTime to JumpBuffer so that, on the next frame, the timer will again be above 0 unless they touch the ground, and therefore they won't be able to use Jump until the timer goes below 0.
        m_LastJumpTime = JumpBuffer;

        float force = jumpForce;

        if (RB.velocity.y < 0)
        {
            //If the player is falling, we add the value (subtract the negative value) of their vertical velocity so that their jump feels bouncier
            force -= RB.velocity.y;
        }

        RB.AddForce(Vector2.up * force, ForceMode2D.Impulse);        
    }

    private IEnumerator StopDashing()
    {
        yield return new WaitForSeconds(m_DashLength);
        m_TrailRenderer.emitting = false;
        m_IsDashing = false;
        RB.velocity = Vector2.zero;
    }

    private bool IsGrounded()
    {
        //Is the player grounded? Or does the player still have some afloat time? If not, they cannot jump.
        if (Physics2D.OverlapBox(m_GroundCheckPoint.position, groundCheckSize, 0, m_GroundLayer) || m_LastOnGroundTime > 0)
        {
            return true;
        } else
        {
            return false;
        }
    }

    private void FlipCharacter()
    {
        Vector3 currentScale = gameObject.transform.localScale;
        currentScale.x *= -1;
        gameObject.transform.localScale = currentScale;

        facingRight = !facingRight;
    }

}
