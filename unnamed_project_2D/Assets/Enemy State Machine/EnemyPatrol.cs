using System;
using Unity.Android.Gradle;
using Unity.VisualScripting;
using UnityEngine;


public class EnemyPatrol : EnemyBaseState
{


private float speed = 3f;
private int facingDirection;
private Transform Eyesight;
private float PatrolDist = 5f;
private float initialdistance;
private float finaldistance;
private float Timer;
public bool enemydetected = false;
private LayerMask playerLayer = 1 << 3; // Assuming Player is on layer 3
private float ColDist = 5f;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void EnterState(Statemanager Enemy)
    {
        Enemy.transform.position = Enemy.EnemySpawnPoint.position;
        Eyesight = Enemy.transform.GetChild(0).transform;
        Debug.Log("Entered Patrol State");
        Enemy.animator.SetBool("Patroling", true);
        Enemy.transform.position = new Vector3(Enemy.transform.position.x, Enemy.transform.position.y, Enemy.transform.position.z);
        facingDirection = (int)Enemy.transform.localScale.x;
        initialdistance = Enemy.EnemySpawnPoint.position.x + PatrolDist;
        finaldistance = Enemy.EnemySpawnPoint.position.x - PatrolDist;
        Debug.Log ("Initial Distance: " + initialdistance);
        Debug.Log ("Final Distance: " + finaldistance);
        Timer = 10f;
    }
    public override void UpdateState(Statemanager Enemy)
    {
        enemyMove(Enemy);
       TimeCounter(Enemy);
       DetectEnemy(Enemy);
    }
    
    private void enemyMove(Statemanager Enemy)
    {
        if(Timer > 0f)
        {
        float checkdist   = Vector2.Distance(Enemy.transform.position, new Vector2(finaldistance, Enemy.transform.position.y));
        if(Mathf.Abs(Enemy.transform.position.x - initialdistance) <= 0.1f && facingDirection == 1)
        {
            facingDirection *= -1;
            Enemy.transform.localScale = new Vector3(facingDirection, Enemy.transform.localScale.y, Enemy.transform.localScale.z);

            {
            Enemy.rb.linearVelocity = new Vector2(speed * facingDirection, Enemy.rb.linearVelocity.y);
            }
        }
        else if(checkdist <= 0.1f && facingDirection == -1)
        {
            facingDirection *= -1;
            Enemy.rb.linearVelocity = new Vector2(speed * facingDirection, Enemy.rb.linearVelocity.y);
            Enemy.transform.localScale = new Vector3(facingDirection, Enemy.transform.localScale.y, Enemy.transform.localScale.z);
        }
        else
        {
            Enemy.rb.linearVelocity = new Vector2(speed * facingDirection, Enemy.rb.linearVelocity.y);
        }
        }
    }
        
    private void TimeCounter(Statemanager Enemy)
    {   Timer -= Time.deltaTime;
        float checkdists   = Enemy.transform.position.x - (initialdistance - PatrolDist);
        if (Timer <= 0.1f)
        {
        if(checkdists < -0.15f)
        {
            if(facingDirection == -1)
             {   facingDirection *= -1;
            Enemy.transform.localScale = new Vector3(facingDirection, Enemy.transform.localScale.y, Enemy.transform.localScale.z);
            Enemy.rb.linearVelocity = new Vector2(speed * facingDirection, Enemy.rb.linearVelocity.y);
             }
        }
        else if(checkdists > 0.15f)
        {
            if(facingDirection == 1)
            {
            facingDirection *= -1;
             Enemy.transform.localScale = new Vector3(facingDirection, Enemy.transform.localScale.y, Enemy.transform.localScale.z);
            Enemy.rb.linearVelocity = new Vector2(speed * facingDirection, Enemy.rb.linearVelocity.y);
            }
        }       
        else if (Timer <= 0f && Mathf.Abs(checkdists) <= 0.15f)
        {   
            Timer = 10f;
            Enemy.transform.localScale = new Vector3(-1, Enemy.transform.localScale.y, Enemy.transform.localScale.z);
            Enemy.animator.SetBool("Patroling", false);
            Enemy.SwitchState(Enemy.EnemyIdle);
        }
        }
    }



    private void DetectEnemy(Statemanager Enemy)
    {
        RaycastHit2D Detect = Physics2D.Raycast(Eyesight.transform.position, Vector2.left*new Vector2(-facingDirection, 0f), ColDist, playerLayer);
        if(Detect.collider != null)
        {
            Debug.DrawRay(Eyesight.transform.position, Vector2.left*new Vector2(-facingDirection, 0f) * ColDist , Color.red);
            Enemy.animator.SetBool("Patroling", false);
            Enemy.SwitchState(Enemy.EnemyAttack);
        } 
        else if(Detect.collider == null)
        {
            Debug.DrawRay(Eyesight.transform.position, Vector2.left*new Vector2(-facingDirection, 0f) * ColDist , Color.green);
        }
    }

}

