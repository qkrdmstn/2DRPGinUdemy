using UnityEngine;

public class Player : Entity
{
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

    [Header("Attack info")]
    private float comboTimeWindow;
    private float comboTime = 0.3f;
    private bool isAttacking;
    private int comboCounter;


    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {

        base.Update();

        CheckInput();
        Movement();
        AnimationCtrl();
        FlipController();

        dashTimer -= Time.deltaTime;
        dashCoolDownTimer -= Time.deltaTime;
        comboTimeWindow -= Time.deltaTime;

    }

    public void AttackOver()
    {
        isAttacking = false;
        comboCounter++;

        if (comboCounter > 2)
            comboCounter = 0;
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

        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            StartAttack();
        }


    }
    void StartAttack()
    {
        if (!isGround)
            return;
        if (comboTimeWindow < 0)
            comboCounter = 0;

        isAttacking = true;
        comboTimeWindow = comboTime;
    }

    private void Dash()
    {
        if (dashCoolDownTimer < 0 && !isAttacking)
        {
            dashCoolDownTimer = dashCoolDown;
            dashTimer = dashDuration;
        }
    }

    private void Movement()
    {

        if (isAttacking)
            rb.velocity = new Vector2(0, 0);
        else if (dashTimer > 0)
            rb.velocity = new Vector2(facingDir * dashSpeed, 0);
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

        animator.SetBool("isAttacking", isAttacking);
        animator.SetInteger("comboCounter", comboCounter);
    }



    private void FlipController()
    {
        if (rb.velocity.x > 0 && !facingRight)
            Flip();
        else if (rb.velocity.x < 0 && facingRight)
            Flip();
    }

}
