using UnityEngine;

public class ChaseState : EnemyBaseState
{
    public ChaseState(EnemyContext context, EnemyManager.EnemyState key, float attackRange, float delay) 
        : base(context, key)
    {
        this.attackRange = attackRange;
        this.delay = delay;
    }

    private Transform target;
    private float attackRange;
    private float delay;
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
    }

    public override void ExitState()
    {
    }

    public override void UpdateState()
    {
        var agent = context.GetAgent();
        var animator = context.GetAnimator();
        
        currentDelay += Time.deltaTime;
        if (currentDelay >= delay)
        {
            if (agent.remainingDistance > agent.stoppingDistance)
            {
                agent.SetDestination(target.position);
            }
            currentDelay = 0;
        }
        
        //animator.SetFloat("forward", agent.velocity.magnitude);
    }

    public override EnemyManager.EnemyState GetNextState()
    {
        var agent = context.GetAgent();
        var status = context.GetMyStatus();
        if (!status.IsAlive()) return EnemyManager.EnemyState.Death;

        if (status.HeavyFlinch()) return EnemyManager.EnemyState.Damage;
        else if (status.Flinch()) return EnemyManager.EnemyState.Damage;

        if (agent.remainingDistance <= agent.stoppingDistance) return EnemyManager.EnemyState.Attack;

        return EnemyManager.EnemyState.Chase;
    }
}
