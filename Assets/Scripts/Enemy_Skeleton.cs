using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Skeleton : Entity
{
    private bool isAttack;

    [Header("Move info")]
    [SerializeField] private float moveSpeed;

    [Header("Player detection")]
    [SerializeField] private float playerCheckDistance;
    [SerializeField] private LayerMask Player;

    private RaycastHit2D isPlayerDetected;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();


        if (isPlayerDetected)
        {
            if (isPlayerDetected.distance > 1)
            {
                rb.velocity = new Vector2(moveSpeed * 3 * facingDir, rb.velocity.y);

                Debug.Log("I see the player");
                isAttack = false;
            }
            else
            {
                Debug.Log("attack" + isPlayerDetected.collider.gameObject.name);
                isAttack = true;
            }
        }
        Debug.Log(rb.velocity);

        if (!isGround || isWall)
        {
            Flip();
        }

        Movement();
    }

    private void Movement()
    {
        if(!isAttack)
            rb.velocity = new Vector2(moveSpeed * facingDir, rb.velocity.y);
    }

    protected override void CollisionChecks()
    {
        base.CollisionChecks();

        isPlayerDetected = Physics2D.Raycast(transform.position, Vector2.right, playerCheckDistance * facingDir, Player);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x + playerCheckDistance * facingDir, transform.position.y));
    }
}
