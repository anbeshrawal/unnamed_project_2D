using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class Player : BaseScript
{
    private Animator pAnimator;
    private Rigidbody2D rb;

  
    [SerializeField] private float moveSpeed;
    [SerializeField] private float maxMoveSpeed = 10f;
    [SerializeField] private float acceleration = 2f;
    [SerializeField] private float deceleration = 4f;
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
        time = Time.deltaTime;
    }
    private void Update()
    {
        HandleInput();
        HandleMovement();
        AttemptAttack();
        HandleAnimations();
        HandleFlip();
    }
    protected virtual void AttemptAttack()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            pAnimator.SetTrigger("attack1");
            Debug.Log("Player Attack!");
            
        }
        
    }

    private void HandleInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        //Debug.Log("Player: HandleInput called " + xInput);
    }
    private void HandleMovement()
    { 
    
        if( canmove == true && xInput != 0)
        { 
        isMoving = true;
        for( float i = moveSpeed;i < maxMoveSpeed; i+= acceleration * Time.deltaTime)
            {
                rb.linearVelocity = new Vector2(xInput * i , rb.linearVelocity.y);
                moveSpeed = i;
            }
        }
        else if (xInput == 0 && isMoving == true)
        {   for ( float j = moveSpeed; j > 0; j -= deceleration * Time.deltaTime)
            {
                rb.linearVelocity = new Vector2(xInput * j, rb.linearVelocity.y);
            }
            isMoving = false;
        }
        
        
    }

    protected void HandleAnimations()
    {
        //pAnimator.SetFloat("yVelocity", rb.linearVelocity.y);
        pAnimator.SetFloat("xVelocity", rb.linearVelocity.x);
        //pAnimator.SetBool("isGrounded", isGrounded);
    
    }

protected virtual void HandleFlip()
    {   
        if (rb.linearVelocity.x > 0 && isFacingRight==false)
        {
            Flip();
        }
        else if (rb.linearVelocity.x < 0 && isFacingRight==true)
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
    
}