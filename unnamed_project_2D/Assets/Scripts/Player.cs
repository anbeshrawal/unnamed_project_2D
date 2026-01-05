using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class Player : BaseScript
{
    private Animator pAnimator;
    private Rigidbody2D rb;

  
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float maxMoveSpeed = 10f;
    [SerializeField] private float acceleration = 2f;
    [SerializeField] private float deceleration = 3f;
    [SerializeField] private bool canmove = true;  
    [SerializeField] private float time;
    private float xInput;
    private bool isMoving = false;

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
        moveSpeed += acceleration * Time.deltaTime;
        }
        else if (xInput == 0 && isMoving == true)
        {   moveSpeed -= deceleration * Time.deltaTime;
            isMoving = false;
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity  = new Vector2(Mathf.Clamp(xInput * moveSpeed, -maxMoveSpeed, maxMoveSpeed), rb.linearVelocity.y);
    }

    protected void HandleAnimations()
    {
        //pAnimator.SetFloat("yVelocity", rb.linearVelocity.y);
        pAnimator.SetFloat("xVelocity", rb.linearVelocity.x);
        //pAnimator.SetBool("isGrounded", isGrounded);
    
    }

}
