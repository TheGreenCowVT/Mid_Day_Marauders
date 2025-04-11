using UnityEngine;
using UnityEngine.AI;

public class IdleState : EnemyBaseState
{
    public IdleState(EnemyContext context, EnemyManager.EnemyState key) : base(context, key)
    {
        
    }

    public override void EnterState()
    {
        //Debug.Log("IDLE STATE");
        var agent = _context.GetAgent();
        var animator = _context.GetAnimator();
        
        agent.isStopped = true;
        animator.CrossFade("Locomotion", 0.02f);
        animator.SetFloat("forward", 0);
        animator.SetFloat("horizontal", 0);
    }

    public override void UpdateState()
    {
        var detector = _context.GetTargetDetector();
    }

    public override EnemyManager.EnemyState GetNextState()
    {
        var detector = _context.GetTargetDetector();
        var status = _context.GetMyStatus();
        if (!status.IsAlive()) return EnemyManager.EnemyState.Death;

        if (status.HeavyFlinch()) return EnemyManager.EnemyState.Damage;
        else if (status.Flinch()) return EnemyManager.EnemyState.Damage;

        if (!detector.HasTarget()) return EnemyManager.EnemyState.Chase;
        
        return EnemyManager.EnemyState.Idle;
    }
}
