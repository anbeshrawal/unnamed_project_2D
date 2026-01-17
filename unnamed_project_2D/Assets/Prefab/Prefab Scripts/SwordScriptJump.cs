using UnityEngine;

public class SwordScriptJump : MonoBehaviour
{
    private float SwordSpeed = 20f;
    [SerializeField] private float destroyTime = 5f;
    [SerializeField] private LayerMask WheretoStick;
    [SerializeField]public bool isStuck = false;
    [SerializeField]private int SwordCount = 1;
    public Vector2 getPoint;
    private Rigidbody2D rb;
    void Start()
    {
      rb = GetComponent<Rigidbody2D>();
      SetVelocity(); 
      SetDestroyTime();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(((1 << collision.gameObject.layer) & WheretoStick) != 0)
        {
            rb.linearVelocity = Vector2.zero;
            if(rb.linearVelocity == Vector2.zero)
            {
                isStuck = true;
            }
            getPoint = rb.transform.position;
            Debug.Log(getPoint);
        }
    }

    // Update is called once per frame
    void SetVelocity()
    {
        if(SwordCount == 1)
        {
            rb.linearVelocity = transform.right * SwordSpeed;
            SwordCount++;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void SetDestroyTime()
    {
        if(isStuck == false)
        {
            SwordCount = 1;
            Destroy(gameObject, destroyTime);
        }
        else 
        return;
    }
}
