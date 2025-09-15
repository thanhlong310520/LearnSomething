using UnityEngine;

public class PlayerCtr : MonoBehaviour
{
    [SerializeField] Rigidbody2D rig;
    [SerializeField] Animator anim;
    [SerializeField] Transform groundCheck;
    [SerializeField] Transform wallCheck;

    [SerializeField] int amountOfJump = 2;
    
    [SerializeField] float movementSpeed = 5;
    [SerializeField] float jumpForce = 10;
    [SerializeField] float groundCheckRadius = 0.1f;
    [SerializeField] float wallCheckDistance = 0.5f;
    [SerializeField] float wallSlideSpeed = 0.5f;
    [SerializeField] float movementForceInAir = 4;
    [SerializeField] float airDragMultiplier = 0.75f;
    [SerializeField] float variableJumpHeightMultiplier = 0.95f;

    [SerializeField] LayerMask whatIsGround;
    [SerializeField] float movementInputDirection;
    [SerializeField] bool isFacingRight = true;
    [SerializeField] bool isWalking = false;
    [SerializeField] bool isWallSliding;
    [SerializeField] bool isGround;
    [SerializeField] bool isTouchingWalls;
    [SerializeField] bool canJump;

    int amountOfJumpLeft = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
        CheckMovementDirection();
        UpdateAnimation();
        CheckIfCanJump();
        CheckIfWallSliding();
    }
    private void FixedUpdate()
    {
        ApplyMovement();
        CheckSurroundings();
    }
    #region Move
    void CheckInput()
    {
        movementInputDirection = Input.GetAxisRaw("Horizontal");
        isWalking = movementInputDirection != 0 ? true : false;
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Jump();
        }
        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            rig.linearVelocityY = rig.linearVelocityY * variableJumpHeightMultiplier;
        }

    }
    void CheckMovementDirection()
    {
        if (isFacingRight && movementInputDirection < 0)
        {
            Flip();
        }
        if (!isFacingRight && movementInputDirection > 0)
        {
            Flip();
        }
    }
    void CheckSurroundings()
    {
        isGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
        isTouchingWalls = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, whatIsGround);
    }
    void CheckIfCanJump()
    {
        if (isGround && rig.linearVelocityY <= 0)
        {
            amountOfJumpLeft = amountOfJump;
        }
        canJump = amountOfJumpLeft > 0 ? true : false;
    }

    void CheckIfWallSliding()
    {
        if (isTouchingWalls && rig.linearVelocityY < 0 && !isGround) isWallSliding = true;
        else isWallSliding = false;
    }
    void UpdateAnimation()
    {
        anim.SetBool("isWalking", isWalking);
        anim.SetBool("isGround", isGround);
        anim.SetBool("isWallSliding", isWallSliding);
        anim.SetFloat("yVelocity", rig.linearVelocityY);
    }
    void ApplyMovement()
    {
        if (isGround)
        {
            rig.linearVelocity = new Vector2(movementSpeed * movementInputDirection, rig.linearVelocity.y);
        }
        else if(!isGround && !isWallSliding && movementInputDirection != 0)
        {
            Vector2 forceToAdd = new Vector2(movementInputDirection * movementForceInAir, 0);
            rig.AddForce(forceToAdd);  

            if(Mathf.Abs(rig.linearVelocityX) > movementSpeed)
            {
                rig.linearVelocityX = movementSpeed * movementInputDirection;
            }
        }

        else if(!isGround && !isWallSliding && movementInputDirection == 0)
        {
            rig.linearVelocityX = rig.linearVelocity.x * airDragMultiplier;
        }
        if(isWallSliding)
        {
            if(rig.linearVelocityY < 0 && -rig.linearVelocityY > wallSlideSpeed)
            {
                rig.linearVelocityY = -wallSlideSpeed;
            }
        } 
    }
    void Jump()
    {
        if (canJump)
        {
            rig.linearVelocity = new Vector2(rig.linearVelocity.x, jumpForce);
            amountOfJumpLeft--;
        }
    }
    void Flip()
    {
        if (!isWallSliding)
        {
            isFacingRight = !isFacingRight;
            transform.Rotate(0, 180, 0);
        }
        
    }
    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position,groundCheckRadius);  
        Gizmos.DrawLine(wallCheck.position,new Vector3(wallCheck.position.x +wallCheckDistance,wallCheck.position.y));
    }
}
