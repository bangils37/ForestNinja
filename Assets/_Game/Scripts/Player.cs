using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;

    [SerializeField] private LayerMask groundLayer; /// để như này dễ sửa thông số của ground
    [SerializeField] private Animator anim;
    [SerializeField] private float speed = 5;
    [SerializeField] private float jumpForce = 350;
    [SerializeField] private string currentAnimName;

    private bool isGrounded = false;
    private bool isJumping = false;
    private bool isAttack = false;

    private float horizontal;

    void Update()           
    {
        isGrounded = CheckGrounded();

        horizontal = Input.GetAxisRaw("Horizontal");

        //if (isAttack)
        //{
        //    rb.velocity = Vector2.zero;
        //    return;
        //}

        if (isGrounded)
        {
            if(isJumping)
            {
                return;
            }

            // jump
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }

            if (Mathf.Abs(horizontal) > 0.01f)
            {
                ChangeAnim("run");
            }

            /// attack
            if (Input.GetKeyDown(KeyCode.C) && isGrounded)
            {
                Attack();
            }

            /// throw
            if (Input.GetKeyDown(KeyCode.V) && isGrounded)
            {
                Throw();
            }
        }

        // check falling 
        if (!isGrounded && rb.velocity.y <= 0)
        {
            Fall();
        }

        // moving
        if (Mathf.Abs(horizontal) > 0.01f)
        {
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);

            transform.rotation = Quaternion.Euler(new Vector3(0, horizontal > 0 ? 0 : 180, 0));
        }
        // idle
        else if (isGrounded)
        {
            ChangeAnim("idle");
            rb.velocity = Vector2.zero;
        }

    }

    private bool CheckGrounded()
    {
        Debug.DrawLine(transform.position, transform.position + Vector3.down * 1.05f, Color.green);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.05f, groundLayer); 

        return (hit.collider != null);
    }

    public void Jump()
    {
        isJumping = true;
        ChangeAnim("jump");
        rb.AddForce(jumpForce * Vector2.up);
    }

    private void Fall()
    {
        isJumping = false;
        ChangeAnim("fall");
    }

    public void Attack()
    {
        ChangeAnim("attack");
    }

    public void Throw()
    {
        ChangeAnim("throw");
    }

    private void ResetAttack()
    {
        isAttack = false;
        currentAnimName = "attack";
        ChangeAnim("idle");
    }

    private void ChangeAnim(string name)
    {
        if (currentAnimName != name)
        {
            anim.ResetTrigger(name);
            currentAnimName = name;
            anim.SetTrigger(currentAnimName);
        }
    }
}
