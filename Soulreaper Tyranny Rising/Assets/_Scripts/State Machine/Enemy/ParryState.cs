using UnityEngine;

public class ParryState : EnemyBaseState
{
    public ParryState(EnemyContext context, EnemyManager.EnemyState key) : base(context, key)
    {
        
    }
    
    private readonly int ParriedHash = Animator.StringToHash("Parried");

    public override void EnterState()
    {
        var agent = _context.GetAgent();
        var animator = _context.GetAnimator();

        agent.isStopped = true;
        agent.Warp(_context.GetTransform().position);
        animator.CrossFade(ParriedHash, 0.02f);
    }
    
    
}
