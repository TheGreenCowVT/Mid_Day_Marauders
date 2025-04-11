using UnityEngine;

public class EnemyBaseState : BaseState<EnemyManager.EnemyState>
{
    public EnemyBaseState(EnemyContext context, EnemyManager.EnemyState key) : base(key)
    {
        this.context = context;
    }

    protected EnemyContext context;
    
    public override void EnterState()
    {
        
    }

    public override void ExitState()
    {
        
    }

    public override void UpdateState()
    {
        
    }

    public override EnemyManager.EnemyState GetNextState()
    {
        return EnemyManager.EnemyState.Idle;
    }

    public override void OnTriggerEnter(Collider other)
    {
        
    }

    public override void OnTriggerExit(Collider other)
    {
        
    }

    public override void OnTriggerStay(Collider other)
    {
        
    }

    public virtual void OnCollisionEnter(Collision collision)
    {
        
    }

    public virtual void OnCollisionExit(Collision collision)
    {
        
    }

    public virtual void OnCollisionStay(Collision collision)
    {
        
    }
}