using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UIElements;

public class EnemyMeleeAttack : MonoBehaviour
{

[SerializeField] private float attackCooldown;

[SerializeField] private BoxCollider2D Collider;
private float cooldownTimer = Mathf.Infinity;
[SerializeField] private float attackrange;
[SerializeField] private float ColDist;
[SerializeField] private float speed;
[SerializeField] private LayerMask playerLayer = 1 << 3; // Assuming Player is on layer 3
[SerializeField] private int damage;
[SerializeField] private Transform Eyesight;
[SerializeField] private int charDirection;
[SerializeField] private bool outofRange = true;

private bool attacking = false;

public GameObject PA;
public GameObject PB; 
private Transform target;
private Transform spawn;
private Animator anim;
private Rigidbody2D rb;


    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        target = PB.transform;
        spawn = PB.transform;
    }

    private void Update()
    {
    cooldownTimer += Time.deltaTime;
    directionCheck();
    DetectEnemy();
    PatrolCheck();
    }
    

    private void directionCheck()
    {
        float x = transform.localScale.x;

        if(x >= 0f)
        {
            charDirection = 1;
        }
        else if(x < 0f)
        {
            charDirection = -1;
        }
    }

    private void DetectEnemy()
    {
        RaycastHit2D Detect = Physics2D.Raycast(Eyesight.transform.position, Vector2.left*new Vector2(-charDirection, 0f), ColDist, playerLayer);
        RaycastHit2D Attack = Physics2D.Raycast(Eyesight.transform.position, Vector2.left*new Vector2(-charDirection, 0f), attackrange, playerLayer);

        if(Detect.collider != null && Attack.collider == null)
        {
            Debug.DrawRay(Eyesight.transform.position, Vector2.left*new Vector2(-charDirection, 0f) * ColDist , Color.red);
            Debug.DrawRay(Eyesight.transform.position, Vector2.left*new Vector2(-charDirection, 0f) * attackrange , Color.blue);
            Debug.Log("Player Detected");
            anim.SetBool("Patroling", false);
            anim.SetBool("EnemySpotted", true);
            //float amount = (rb.linearVelocityX+speed) * Time.deltaTime * Mathf.Sign(rb.linearVelocityX);  
            rb.linearVelocity = new Vector2(speed*charDirection, 0);
            outofRange = false;
            attacking = false;
        } 
        else if(Detect.collider == null && Attack.collider == null)
        {
            Debug.DrawRay(Eyesight.transform.position, Vector2.left*new Vector2(-charDirection, 0f) * ColDist , Color.green);
            Debug.DrawRay(Eyesight.transform.position, Vector2.left*new Vector2(-charDirection, 0f) * attackrange , Color.blue);
            anim.SetBool("EnemySpotted", false);
            Debug.Log("Player Not Detected");
            outofRange = true;
            attacking = false;
    
        }
        else if(Detect.collider != null && Attack.collider != null)
        {
            EAttack();
        }

        void EAttack()
        {
            rb.linearVelocity = new Vector2(0, 0);
            if(cooldownTimer >= attackCooldown)
            {
                attacking = true;
            Debug.Log("Player in Attack Range");
             anim.SetBool("Patroling", false);
            anim.SetBool("EnemySpotted", false);
            anim.SetTrigger("Attack1");
            cooldownTimer = 0;
            }
        }}

        private void DamagePlayer()
        {
            Debug.Log("Player Damaged");
        }

        private void PatrolCheck()
        {
            Debug.Log("Patrol Check");
            //Vector2 P = CurrentPoint.position - transform.position;
            if(anim.GetBool("EnemySpotted") == false && attacking == false)
            {
                float dist = transform.position.x - target.position.x;
            
            if(Mathf.Abs(dist) <= 0.2f)
            {
            if(target == PB.transform)
            {
                target = PA.transform;
                movetoTarget();
            }
            else if(target == PA.transform)
            {
                target = PB.transform;
                movetoTarget();
            } 
            }
            else if(Mathf.Abs(dist) > 0.2f && Mathf.Abs(dist) < 6f)
            {
                Debug.Log(dist);
                movetoTarget();
            }
            else if(Mathf.Abs(dist) >= 6f)
            {
                Debug.Log("Out of Range - Resetting Position");
                if(outofRange  ==  true && target == PA.transform)
                {
                    Debug.Log(dist);
                    anim.SetBool("Patroling", false);
                    rb.linearVelocity = new Vector2(0, 0);
                    Invoke("RangeCheck", 1.5f);
                }
                else if(outofRange  ==  true && target == PB.transform)
                {
                    Debug.Log(dist);
                    anim.SetBool("Patroling", false);
                    rb.linearVelocity = new Vector2(0, 0);
                    Invoke("RangeCheck", 1.5f);
            }
        }
        }
        }


    private void movetoTarget()
    {
        if(target == PA.transform)
        {
        Debug.Log("Moving to A");
         anim.SetBool("Patroling", true);
        rb.linearVelocity = new Vector2(-speed, 0);
        transform.localScale = new Vector3(-1, 1, 1);
        }
        else if(target == PB.transform)
        {
            Debug.Log("Moving to B");
             anim.SetBool("Patroling", true);
        rb.linearVelocity = new Vector2(speed, 0);
        transform.localScale = new Vector3(1, 1, 1);
        }
        else 
        {
            anim.SetBool("Patroling", false);
        }

}

private void RangeCheck()
    {
        Debug.Log("Resetting Position");

        transform.position = new Vector3(spawn.position.x - 0.5f, transform.position.y, transform.position.z);
 }
}


    


