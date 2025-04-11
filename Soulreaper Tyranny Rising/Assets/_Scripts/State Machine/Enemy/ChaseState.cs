using UnityEngine;

public class ChaseState : EnemyBaseState
{
    public ChaseState(EnemyContext context, EnemyManager.EnemyState key, float attackRange) 
        : base(context, key)
    {
        _attackRange = attackRange;
    }

    private Transform _target;
    private float _attackRange;
    private float _delay = 0.25f;
    private float _currentDelay;

    public override void EnterState()
    {
        //Debug.Log("CHASE STATE");
        var agent = _context.GetAgent();
        var detector = _context.GetTargetDetector();
        var animator = _context.GetAnimator();
        var transform = _context.GetTransform();
        
        agent.isStopped = false;
        agent.stoppingDistance = _attackRange;
        agent.updatePosition = false;
        agent.updateRotation = true;
        _target = detector.GetTarget();
        agent.SetDestination(_target.position);
        animator.CrossFade("Locomotion", 0.02f);
        _currentDelay = 0;
    }

    public override void ExitState()
    {
    }

    public override void UpdateState()
    {
        var agent = _context.GetAgent();
        var animator = _context.GetAnimator();
        
        _currentDelay += Time.deltaTime;
        if (_currentDelay >= _delay)
        {
            if (agent.remainingDistance > agent.stoppingDistance)
            {
                agent.SetDestination(_target.position);
            }
            _currentDelay = 0;
        }
        
        //animator.SetFloat("forward", agent.velocity.magnitude);
    }

    public override EnemyManager.EnemyState GetNextState()
    {
        var agent = _context.GetAgent();
        var status = _context.GetMyStatus();
        if (!status.IsAlive()) return EnemyManager.EnemyState.Death;

        if (status.HeavyFlinch()) return EnemyManager.EnemyState.Damage;
        else if (status.Flinch()) return EnemyManager.EnemyState.Damage;

        if (agent.remainingDistance <= agent.stoppingDistance) return EnemyManager.EnemyState.Attack;

        return EnemyManager.EnemyState.Chase;
    }
}
