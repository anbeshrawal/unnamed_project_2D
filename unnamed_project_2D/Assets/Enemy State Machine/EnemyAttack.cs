
using System.Threading;
using UnityEngine;

public class EnemyAttack : EnemyBaseState
{
    private float speed = 3f;
    private float timer;
    private int facingDirection;
    private Transform Eyesight;
    public bool enemydetected = true;
    private bool attacking = false;
    private LayerMask playerLayer = 1 << 3; // Assuming Player is on layer 3
    private float attackrange = 3f;
    private float ColDist = 5f;

    private float chase_distance = 8f;
    private bool coolingdown = false;

    public override void EnterState(Statemanager Enemy)
    {
     facingDirection = (int)Enemy.transform.localScale.x; 
    Eyesight = Enemy.transform.GetChild(0).transform; 
    }
    public override void UpdateState(Statemanager Enemy)
    {
    deteckEnemy(Enemy);
    }

    private void deteckEnemy(Statemanager Enemy)
    {
        Enemy.animator.SetBool("EnemySpotted", true);
        Enemy.animator.SetBool("Patroling", false);
        RaycastHit2D Detect = Physics2D.Raycast(Eyesight.transform.position, Vector2.left*new Vector2(-facingDirection, 0f), ColDist, playerLayer);
        DrawColliders(Detect, Enemy);
}

void DrawColliders(RaycastHit2D Detect, Statemanager Enemy)
    {
if(Detect.collider != null)
        {
            Debug.DrawRay(Eyesight.transform.position, Vector2.left*new Vector2(-facingDirection, 0f) * ColDist , Color.red);
            if(Detect.collider.CompareTag("Player"))
            {
                timer = 2f;
                EAttack(Enemy, Detect.collider);
            }
            
        }
        if(Detect.collider == null)
        {
            Debug.DrawRay(Eyesight.transform.position, Vector2.left*new Vector2(-facingDirection, 0f) * ColDist , Color.green);
            BacktoBase(Enemy);
        }
    }
 void EAttack(Statemanager Enemy, Collider2D player)
    {
    float distance = Vector2.Distance(Enemy.transform.position, player.transform.position);
    if(distance <= attackrange)
    {
        Enemy.animator.Play("E_Attack1");
        Enemy.animator.SetBool("EnemySpotted", false);
        Enemy.animator.SetBool("Patroling", false);
    }
    else if (distance > attackrange)
    {
    Enemy.animator.SetBool("Attacking", false);
    PursuePlayer(Enemy, player);
    }
    
    }


 void BacktoBase(Statemanager Enemy)
    {
    Enemy.animator.SetBool("EnemySpotted", false);
    
        if (Enemy.transform.position.x - Enemy.EnemySpawnPoint.position.x < -0.2f)
        {
            facingDirection = 1;
            Debug.Log("F1");     

            Enemy.transform.localScale = new Vector3(facingDirection, Enemy.transform.localScale.y, Enemy.transform.localScale.z);
            Enemy.animator.SetBool("Patroling", true);
            Enemy.rb.linearVelocity = new Vector2(speed * facingDirection, Enemy.rb.linearVelocity.y);
        
        }
        else if (Enemy.transform.position.x - Enemy.EnemySpawnPoint.position.x > 0.2f)
        
        {
            facingDirection = -1;
            Debug.Log("F2");
            Enemy.transform.localScale = new Vector3(facingDirection, Enemy.transform.localScale.y, Enemy.transform.localScale.z);
            Enemy.animator.SetBool("Patroling", true);
            Enemy.rb.linearVelocity = new Vector2(speed * facingDirection, Enemy.rb.linearVelocity.y);
        }
        
        else if (Mathf.Abs(Enemy.transform.position.x - Enemy.EnemySpawnPoint.position.x) <= 0.2f)
        {
            Debug.Log("F3"); 
            Enemy.animator.SetBool("Patroling", false);
            Enemy.SwitchState(Enemy.EnemyIdle);
        }

    }    
void PursuePlayer(Statemanager Enemy, Collider2D player)
    {
        timer -= Time.deltaTime;
        Debug.DrawRay(Eyesight.transform.position, Vector2.left*new Vector2(-facingDirection, 0f) * chase_distance , Color.red);
        if(chase_distance > Vector2.Distance(Enemy.transform.position, player.transform.position))
        {
                Debug.Log("Pursuing Player");
                //Debug.Log(Vector2.Distance(Enemy.transform.position, player.transform.position));
                Enemy.animator.Play("E_run 0");
                Enemy.animator.SetBool("Patroling", false);
                Enemy.animator.SetBool("Attacking", false);

            if (Enemy.transform.position.x - player.transform.position.x<0)
            {
            facingDirection = 1;
            }
             else if (Enemy.transform.position.x - player.transform.position.x > 0)
            {
            facingDirection = -1;
            }
            Enemy.rb.linearVelocity = new Vector2(speed * facingDirection, Enemy.rb.linearVelocity.y);

        }
        else if (chase_distance <= Vector2.Distance(Enemy.transform.position, player.transform.position) && timer <= 0.2f)
         {
            Debug.Log(Vector2.Distance(Enemy.transform.position, player.transform.position));
            BacktoBase(Enemy);
         }
     
    }

}