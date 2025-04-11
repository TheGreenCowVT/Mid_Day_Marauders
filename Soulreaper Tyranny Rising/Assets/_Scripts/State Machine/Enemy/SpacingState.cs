using UnityEngine;

public class SpacingState : EnemyBaseState
{
    public SpacingState(EnemyContext context, EnemyManager.EnemyState key, float attackRange) 
        : base(context, key)
    {
        this.attackRange = attackRange;
    }

    private Transform target;
    private float attackRange;
    private float delay = 0.25f;
    private float currentDelay;

    public override void EnterState()
    {
        //Debug.Log("CHASE STATE");
        var agent = context.GetAgent();
        var detector = context.GetTargetDetector();
        var animator = context.GetAnimator();
        var transform = context.GetTransform();
        
        agent.isStopped = false;
        agent.stoppingDistance = attackRange;
        agent.updatePosition = false;
        agent.updateRotation = true;
        target = detector.GetTarget();
        agent.SetDestination(target.position);
        animator.CrossFade("Locomotion", 0.02f);
        currentDelay = 0;
        animator.SetFloat("forward", -1);
    }

    public override void ExitState()
    {
    }

    public override void UpdateState()
    {
        var animator = context.GetAnimator();
        
        currentDelay += Time.deltaTime;
        if (currentDelay >= delay)
        {
            animator.SetFloat("forward", 0);
        }
    }

    public override EnemyManager.EnemyState GetNextState()
    {
        var agent = context.GetAgent();

        if (currentDelay >= delay) return EnemyManager.EnemyState.Chase;

        return EnemyManager.EnemyState.Spacing;
    }
}
