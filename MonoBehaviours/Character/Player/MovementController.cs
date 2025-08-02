using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    public float dashForce = 10.0f;
    public float dashDuration = 1.0f;
    public float movementSpeed = 3.0f;
    Vector2 movement;

    Animator animator;
    string animationState = "AnimationState";
    private bool isDashing = false;

    Rigidbody2D rb2D;

    enum CharStates
    {
        idle = 1,
        move = 2,
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
       UpdateState();
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            StartCoroutine(DashCoroutine());
        }
    }

    void FixedUpdate()
    {
        MoveCharactor();
    }

    IEnumerator DashCoroutine()
    {
        isDashing = true;
        yield return new WaitForSeconds(dashDuration);
        isDashing = false;
    }

    private void MoveCharactor()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        if (!isDashing)
        {
            rb2D.velocity = movement * movementSpeed;
        }
        else
        {
            rb2D.velocity = movement * dashForce;
        }
    }

    private void UpdateState()
    {
        /*
        // 뒤집기
        if (movement.x > 0.0f)
        {
            transform.localScale = new Vector2(1, 1);
        }
        else if (movement.x < 0.0f)
        {
            transform.localScale = new Vector2(-1, 1);
        }
        */

        // 애니메이션
        if (movement.x != 0 ||  movement.y != 0)
        {
            animator.SetInteger(animationState, (int)CharStates.move);
        }
        else
        {
            animator.SetInteger(animationState, (int)CharStates.idle);
        } 
    }
}
