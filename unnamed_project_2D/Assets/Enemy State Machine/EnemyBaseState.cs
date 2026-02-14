using UnityEngine;

public abstract class EnemyBaseState 
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public abstract void EnterState(Statemanager Enemy);

    // Update is called once per frame
    public abstract void UpdateState(Statemanager Enemy);


}
