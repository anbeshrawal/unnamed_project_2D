using UnityEngine;

public class Camerascript : MonoBehaviour
{
    [SerializeField] private float followspeed;
    [SerializeField] private Transform target;

    void Start()
    {
     transform.position = new Vector3(target.position.x, target.position.y,  -10f);   
    }
    
    void Update()
    {
        
        Vector3 Newpos = new Vector3(target.position.x, target.position.y,  -10f);
        transform.position = Vector3.Lerp(transform.position, Newpos, followspeed * Time.deltaTime);
    }
}
