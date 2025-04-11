using UnityEngine;

public class ParryState : EnemyBaseState
{
    public ParryState(EnemyContext context, EnemyManager.EnemyState key) : base(context, key)
    {
        
    }
    
    private readonly int ParriedHash = Animator.StringToHash("Parried");

    public override void EnterState()
    {
        var agent = context.GetAgent();
        var animator = context.GetAnimator();

        agent.isStopped = true;
        agent.Warp(context.GetTransform().position);
        animator.CrossFade(ParriedHash, 0.02f);
    }
    
    
}
