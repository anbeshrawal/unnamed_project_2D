using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Unity.VisualScripting;

public class BaseScript : MonoBehaviour
{
    [SerializeField]protected Transform attackPoint;
    [SerializeField]protected float attackRadious;
    [SerializeField]protected LayerMask whatisTarget;

    [Header ("Collision")]
    [SerializeField]protected float groundDistance;
    [SerializeField]protected LayerMask whatisGround;
    [SerializeField] protected bool isGrounded;
    [SerializeField] protected int health;



    private void Start()
    {
        Debug.Log("BaseScript: Start called");
    }

    public void damageTarget()
    {
        Collider2D[] ecolliders = Physics2D.OverlapCircleAll(attackPoint.position, attackRadious, whatisTarget);
        foreach (Collider2D Enemy in ecolliders)
        {
            BaseScript Target = Enemy.GetComponent<BaseScript>();
            Target.TakeDamage();

        }
        Debug.Log("BaseScript: damageEnemy called");
    }
    public virtual void TakeDamage()
    {
        Debug.Log("BaseScript: takeDamage called");
    }
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRadious);
    }

    protected void Collision()
    {
        Debug.Log("BaseScript: Collision called"); 
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(0, -groundDistance));
    }

    protected virtual void CheckCollision()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundDistance, whatisGround);

    }
}



