using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
private void damage()
    {   
        BaseScript baseScript = GetComponentInParent<BaseScript>();
        baseScript.damageTarget();
        Debug.Log("Damage Target event triggered.");
    }
}
