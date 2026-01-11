using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Unity.VisualScripting;
using System.Data;
using System;
using UnityEngine.InputSystem;
using NUnit.Framework;

public class Player : BaseScript
{
    private Animator pAnimator;
    private Rigidbody2D rb;

    //[SerializeField] private Vector2 moveSpeed;
    //[SerializeField] private float acceleration = 3f;
    //[SerializeField] private float deceleration = 3f;
    [SerializeField] private bool canmove = true;  
    [SerializeField] private float time;
    [SerializeField]private GameObject edgePoint;
    private float xInput;
    //private bool isMoving = false;
    private int facingDirection = 1;
    private bool isFacingRight = true;
    [SerializeField] private float maxMoveSpeed = 8f;
    [SerializeField] private float velPower = 1.5f;
    [SerializeField] private float frictionAmount = 0.1f;
    [SerializeField] private float jumpSpeed = 7f;
    [SerializeField] private float SJF = 0.6f; // Second Jump Force multiplier
    [SerializeField] private int jumpCount = 0;
    [SerializeField]private bool canJump = false;
    [SerializeField] private int totaljumps = 2;
    private float move;
    [SerializeField]private bool airborne = false;
    [SerializeField]private float jumpmovespeed = 3f;
    private float targetSpeed;
    private float a_move;
    

    private void Start()
    {
        pAnimator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        
    }
    private void Update()
    {
        HandleInput();
        HandleAnimations();
        //HandleMovement();
        CheckCollision();
        movement();
        AttemptAttack();
        HandleFlip();
        handleAirborne();
    }

    private void HandleInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");

    if (Input.GetKeyDown(KeyCode.Space))
    {
        Jump();
    }
    }


#region  Jump

    void handleAirborne()
    {
        if(isGrounded == false)
        {
            airborne = true;
            pAnimator.SetBool("canJump", !isGrounded);
        }
        else
        {
            airborne = false;
        }
    }
    void Jump()
    {
        if(canJump && jumpCount < totaljumps)
        {  
        pAnimator.SetBool("canJump", canJump);
        jumpCount++;
        if(jumpCount == 0)
                {
                    rb.AddForce(jumpSpeed * Vector2.up, ForceMode2D.Impulse);
                    Debug.Log("J1");
                }

        else if(jumpCount == 1)
                {
                    rb.AddForce(SJF*jumpSpeed * Vector2.up, ForceMode2D.Impulse);
                    Debug.Log("J2");
                }
        else if(jumpCount == 2)
        {                
            transform.position = Vector2.MoveTowards(transform.position, edgePoint.transform.position, 3f);
            Debug.Log("Edge Jumped");
        } 
        
        }
    }

    /*void JumpCheck()
    {
        if (isGrounded)
        {
            canJump = true;
            canmove = true;
            jumpCount = 0f;
        }
    if(totaljumps > jumpCount && isGrounded == true)
        {
            canJump = true;
            Jump();
        }
        else if(totaljumps >= jumpCount && !isGrounded)
        {
            canJump = true;
            Jump();
        }
        else if(totaljumps <= jumpCount && !isGrounded)
        {
            canJump = false;
        }
        else if(totaljumps <= jumpCount && isGrounded)
        {
            jumpCount = 0f;
        }
    }*/

    #endregion
    
    #region Movement

    override protected void CheckCollision()
    {
        base.CheckCollision();
        if (isGrounded)
        {   pAnimator.SetBool("canJump", !isGrounded);
            airborne = false;
            canJump = true;
            jumpCount = 0;
        }
    }

    private void movement()
    {   
        if(canmove == true && isGrounded == true)
        {
        targetSpeed = xInput * maxMoveSpeed;
        float speeddiff = targetSpeed - rb.linearVelocity.x;
        //float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : deceleration;
        
        if(targetSpeed == 0)
        {
            float amount = Mathf.Min(Mathf.Abs(rb.linearVelocity.x), Mathf.Abs(frictionAmount));
            amount *= Mathf.Sign(rb.linearVelocity.x);
            rb.AddForce(-amount * Vector2.right, ForceMode2D.Impulse);

        }
        else
        {
            move = Mathf.Pow(Mathf.Abs(speeddiff), velPower) * Mathf.Sign(speeddiff);

            rb.AddForce(move * Vector2.right);
        }
        }

        if(canmove == true && airborne == true)
        {
        targetSpeed = xInput * jumpmovespeed;
        float airspeeddiff = targetSpeed - rb.linearVelocity.x;
        
        if(targetSpeed == 0)
        {
            float amount = Mathf.Min(Mathf.Abs(rb.linearVelocity.x), Mathf.Abs(frictionAmount));
            amount *= Mathf.Sign(rb.linearVelocity.x);
            rb.AddForce(-amount * Vector2.right, ForceMode2D.Impulse);

        }
        else
        {
            a_move = Mathf.Pow(Mathf.Abs(airspeeddiff), velPower) * Mathf.Sign(airspeeddiff);

            rb.AddForce(a_move * Vector2.right);
        }
        }
    }



#region useless code
//old movement method
    /*private void HandleMovement()
    { 
         HandleFlip();
        if( canmove == true && xInput != 0)
        { 
        isMoving = true;
        Vector2 targetVelocity = new Vector2(xInput, 0) * maxMoveSpeed;
        moveSpeed = Vector2.Lerp(moveSpeed, targetVelocity, acceleration * Time.deltaTime); 
        rb.linearVelocity = new Vector2(moveSpeed.x, rb.linearVelocity.y);
        
        
        }
        else if (xInput == 0 && isMoving == true)
        {   
        Vector2 targetVelocity = new Vector2(0, 0);
        moveSpeed = Vector2.Lerp(moveSpeed, targetVelocity, deceleration * Time.deltaTime); 
        rb.linearVelocity = new Vector2(moveSpeed.x, rb.linearVelocity.y);
        }    
    }*/

#endregion

#region Flip and Animations
protected virtual void HandleFlip()
    {   
        if (move > 0 && isFacingRight==false && targetSpeed !=0)
        {
            Flip();
        }
        else if (move < 0 && isFacingRight==true && targetSpeed !=0)
        {
            Flip();
        }
    }
private void Flip()
    {
        facingDirection = facingDirection * -1;
        transform.Rotate(0,180,0);
        isFacingRight = !isFacingRight;
    }
  #endregion  
    protected virtual void AttemptAttack()
    {
        canmove = false;
        if (Input.GetKeyDown(KeyCode.Mouse0) && isGrounded)
        {
            pAnimator.SetTrigger("attack1");
            
            
        }
        else
        {
            canmove = true;
        }
    }

    private void FixedUpdate()
    {
        
    }

    protected void HandleAnimations()
    {
        float Factor;
    if(rb.linearVelocity.y > 0.1f)
        {
            Factor = 1f; // Jumping
        }
        else if (rb.linearVelocity.y < -0.1f)
        {
            Factor = -1f; // Falling
        }
        else
        {
            Factor = 0f; // Grounded
        }
        pAnimator.SetBool("isGrounded", isGrounded);
        if(isGrounded)
        pAnimator.SetFloat("xVelocity", rb.linearVelocity.x);

        pAnimator.SetFloat("yVelocity", Factor);
    
    }

    #endregion
}