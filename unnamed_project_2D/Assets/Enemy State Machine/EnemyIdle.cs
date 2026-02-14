using Unity.VisualScripting;
using UnityEngine;

public class EnemyIdle : EnemyBaseState

{

#region Variables
private float Timer;
public bool enemydetected = false;
private LayerMask playerLayer = 1 << 3; // Assuming Player is on layer 3
private float ColDist = 5f;
private int facingDirection;
private Transform Eyesight;
public GameObject target;
public bool outofRange = true;

#endregion

        public override void EnterState(Statemanager Enemy)
    {
        Eyesight = Enemy.transform.GetChild(0).transform;
        facingDirection = (int)Enemy.transform.localScale.x;
        Enemy.transform.position = Enemy.EnemySpawnPoint.position;
        Enemy.animator.Play("E_Idle");
        Debug.Log("Entered Idle State");
        Timer = 6f;
    }
    public override void UpdateState(Statemanager Enemy)
    {
        Timer -= Time.deltaTime;
        DetectEnemy(Enemy);      
        if (Timer <= 0)
        {
            Timer = 6f;
            Enemy.SwitchState(Enemy.EnemyPatrol);
            
        }   
        }



    private void DetectEnemy(Statemanager Enemy)
    {
        RaycastHit2D Detect = Physics2D.Raycast(Eyesight.transform.position, Vector2.left*new Vector2(-facingDirection, 0f), ColDist, playerLayer);
        if(Detect.collider != null)
        {
            Debug.DrawRay(Eyesight.transform.position, Vector2.left*new Vector2(-facingDirection, 0f) * ColDist , Color.red);
            Enemy.animator.SetBool("EnemySpotted", true);
            Enemy.SwitchState(Enemy.EnemyAttack);

        } 
        if(Detect.collider == null)
        {
            Debug.DrawRay(Eyesight.transform.position, Vector2.left*new Vector2(-facingDirection, 0f) * ColDist , Color.green);
        }
    }

}


