using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Unity.VisualScripting;
using System;
public class Player : BaseScript
{
    private Animator pAnimator;
    private Rigidbody2D rb;

  
    //[SerializeField] private Vector2 moveSpeed;
    [SerializeField] private float acceleration = 3f;
    [SerializeField] private float deceleration = 3f;
    [SerializeField] private bool canmove = true;  
    [SerializeField] private float time;
    private float xInput;
    private bool isMoving = false;
    private int facingDirection = 1;
    private bool isFacingRight = true;
    [SerializeField] private float maxMoveSpeed = 8f;
    [SerializeField] private float velPower = 1.5f;
    [SerializeField] private float frictionAmount = 0.1f;
    private float move;

    private float targetSpeed;
    

    private void Start()
    {
        pAnimator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        
    }
    private void Update()
    {
        HandleInput();
        //HandleMovement();
        CheckCollision();
        movement();
        AttemptAttack();
        HandleAnimations();
        HandleFlip();
        time = Time.deltaTime;
    }
    protected virtual void AttemptAttack()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            pAnimator.SetTrigger("attack1");
            Debug.Log("Player Attack!");
            canmove = false;
            
        }
        else
        {
            canmove = true;
        }
    }

    private void HandleInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        //Debug.Log("Player: HandleInput called " + xInput);
    }

    private void FixedUpdate()
    {
        
    }

    #region Movement
    private void movement()
    {   
        if(canmove == true && isGrounded == true)
        {
        targetSpeed = xInput * maxMoveSpeed;
        float speeddiff = targetSpeed - rb.linearVelocity.x;
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : deceleration;
        
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
    }



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
    protected void HandleAnimations()
    {
        //pAnimator.SetFloat("yVelocity", rb.linearVelocity.y);
        pAnimator.SetFloat("xVelocity", rb.linearVelocity.x);
        //pAnimator.SetBool("isGrounded", isGrounded);
    
    }
}