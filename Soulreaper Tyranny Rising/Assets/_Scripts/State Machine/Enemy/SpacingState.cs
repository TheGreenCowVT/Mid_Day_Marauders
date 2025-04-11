using UnityEngine;

public class SpacingState : EnemyBaseState
{
    public SpacingState(EnemyContext context, EnemyManager.EnemyState key, float attackRange) 
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
        animator.SetFloat("forward", -1);
    }

    public override void ExitState()
    {
    }

    public override void UpdateState()
    {
        var animator = _context.GetAnimator();
        
        _currentDelay += Time.deltaTime;
        if (_currentDelay >= _delay)
        {
            animator.SetFloat("forward", 0);
        }
    }

    public override EnemyManager.EnemyState GetNextState()
    {
        var agent = _context.GetAgent();

        if (_currentDelay >= _delay) return EnemyManager.EnemyState.Chase;

        return EnemyManager.EnemyState.Spacing;
    }
}
