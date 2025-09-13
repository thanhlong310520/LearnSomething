using UnityEngine;

public class PlayerCtr : MonoBehaviour
{
    [SerializeField] Rigidbody2D rig;
    [SerializeField] Animator anim;
    [SerializeField] Transform groundCheck;

    [SerializeField] int amountOfJump = 2;
    
    [SerializeField] float movementSpeed = 5;
    [SerializeField] float jumpForce = 10;
    [SerializeField] float groundCheckRadius = 0.1f;

    [SerializeField] LayerMask whatIsGround;
    [SerializeField] float movementInputDirection;
    [SerializeField] bool isFacingRight = true;
    [SerializeField] bool isWalking = false;
    [SerializeField] bool isGround;
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
        isGround = Physics2D.OverlapCircle(groundCheck.position,groundCheckRadius,whatIsGround);
    }
    void CheckIfCanJump()
    {
        if (isGround && rig.linearVelocityY <= 0)
        {
            amountOfJumpLeft = amountOfJump;
        }
        canJump = amountOfJumpLeft > 0 ? true : false;
    }
    void UpdateAnimation()
    {
        anim.SetBool("isWalking", isWalking);
        anim.SetBool("isGround", isGround);
        anim.SetFloat("yVelocity", rig.linearVelocityY);
    }
    void ApplyMovement()
    {
        rig.linearVelocity = new Vector2(movementSpeed * movementInputDirection, rig.linearVelocity.y);
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
        isFacingRight = !isFacingRight;
        transform.Rotate(0, 180, 0);
        
    }
    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position,groundCheckRadius);  
    }
}
