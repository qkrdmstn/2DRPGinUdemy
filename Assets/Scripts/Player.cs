using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;

    [Header("Movement info")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    private float horizonInput;

    [Header("Dash info")]
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashDuration;
    private float dashTimer;
    [SerializeField] private float dashCoolDown;
    private float dashCoolDownTimer;

    [Header("Collision info")]
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask ground;
    private bool isGround;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        CheckInput();
        Movement();
        AnimationCtrl();
        PlayerFlip();
        CollisionChecks();

        dashTimer -= Time.deltaTime;
        dashCoolDownTimer -= Time.deltaTime;

    }

    private void CheckInput()
    {
        //Move Input
        horizonInput = Input.GetAxisRaw("Horizontal");

        //Jump Input
        if (Input.GetKeyDown(KeyCode.Space))
            Jump();

        //Dash Input
        if (Input.GetKeyDown(KeyCode.LeftShift))
            Dash();
    }

    private void Dash()
    {
        if (dashCoolDownTimer < 0)
        {
            dashCoolDownTimer = dashCoolDown;
            dashTimer = dashDuration;
        }
    }

    private void Movement()
    {
        if (dashTimer > 0)
            rb.velocity = new Vector2(horizonInput * dashSpeed, 0);
        else
            rb.velocity = new Vector2(horizonInput * moveSpeed, rb.velocity.y);
    }

    private void Jump()
    {
        if (isGround)
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    private void AnimationCtrl()
    {
        bool isMoving;
        if (horizonInput != 0)
            isMoving = true;
        else
            isMoving = false;

        animator.SetBool("isMoving", isMoving);
        animator.SetBool("isGround", isGround);

        animator.SetFloat("yVelocity", rb.velocity.y);

        animator.SetBool("isDash", dashTimer > 0);
    }

    private void PlayerFlip()
    {
        if (rb.velocity.x > 0)
            transform.rotation = Quaternion.Euler(0, 0, 0);
        else if (rb.velocity.x < 0)
            transform.rotation = Quaternion.Euler(0, 180, 0);
    }

    private void CollisionChecks()
    {
        isGround = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, ground);
        Debug.DrawRay(transform.position, Vector2.down * groundCheckDistance, new Color(0, 1, 0));
    }
}
