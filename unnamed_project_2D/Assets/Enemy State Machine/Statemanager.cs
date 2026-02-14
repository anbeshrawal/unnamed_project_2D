using UnityEngine;

public class Statemanager : MonoBehaviour
{
  EnemyBaseState currentState;
  public EnemyAttack EnemyAttack = new EnemyAttack();
  public EnemyIdle EnemyIdle = new EnemyIdle();
  public EnemyPatrol EnemyPatrol = new EnemyPatrol();
  public EnemyHurtDeath EnemyHurtDeath = new EnemyHurtDeath(); 

    public Animator animator;
    public Rigidbody2D rb;
  [SerializeField] public Transform EnemySpawnPoint;

  int health { get; set; } = 100;
     
    void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
  {
    currentState = EnemyIdle;
    currentState.EnterState(this);
  }

  void Update()
  {
    currentState.UpdateState(this);
  }


public void SwitchState(EnemyBaseState newState)
{
    currentState = newState;
    currentState.EnterState(this);
}

}
