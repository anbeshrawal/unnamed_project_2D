using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Unity.VisualScripting;
using System;
public class Player : BaseScript
{
    private Animator pAnimator;
    private Rigidbody2D rb;

  
    [SerializeField] private Vector2 moveSpeed;
    [SerializeField] private float maxMoveSpeed = 10f;
    [SerializeField] private float acceleration = 3f;
    [SerializeField] private float deceleration = 5f;
    [SerializeField] private bool canmove = true;  
    [SerializeField] private float time;
    private float xInput;
    private bool isMoving = false;
    private int facingDirection = 1;
    private bool isFacingRight = true;
    

    private void Start()
    {
        pAnimator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        
    }
    private void Update()
    {
        HandleInput();
        HandleMovement();
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

    #region Movement
    private void HandleMovement()
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
    }


protected virtual void HandleFlip()
    {   
        if (moveSpeed.x > 0 && isFacingRight==false)
        {
            Flip();
        }
        else if (moveSpeed.x < 0 && isFacingRight==true)
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