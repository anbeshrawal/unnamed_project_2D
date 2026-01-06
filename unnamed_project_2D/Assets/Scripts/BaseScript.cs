using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class BaseScript : MonoBehaviour
{
    [SerializeField]protected Transform attackPoint;
    [SerializeField]protected float attackRadious;
    [SerializeField]protected LayerMask whatisTarget;

    private Rigidbody2D rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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
}



