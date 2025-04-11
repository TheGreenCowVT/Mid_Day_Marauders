using UnityEngine;
using UnityEngine.AI;

public class FocusIdleState : EnemyBaseState
{
    public FocusIdleState(EnemyContext context, EnemyManager.EnemyState key) : base(context, key)
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
        var target = _context.GetTargetDetector().GetTarget();
        var transform = _context.GetTransform();

        // Get direction to target(ignoring Y - axis difference)
        Vector3 direction = target.position - transform.position;
        direction.y = 0; // Lock rotation to Y-axis only

        if (direction != Vector3.zero)
        {
            // Get target rotation
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            // Smoothly rotate towards target rotation (Y-axis only)
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }

    public override EnemyManager.EnemyState GetNextState()
    {
        var detector = _context.GetTargetDetector();
        var status = _context.GetMyStatus();
        var chase = _context.UsingState(EnemyManager.EnemyState.Chase);


        if (!status.IsAlive()) return EnemyManager.EnemyState.Death;

        if (status.HeavyFlinch()) return EnemyManager.EnemyState.Damage;
        else if (status.Flinch()) return EnemyManager.EnemyState.Damage;

        if (chase)
        {
            if (detector.HasTarget()) return EnemyManager.EnemyState.Chase;
        }

        return EnemyManager.EnemyState.FocusIdle;
    }
}
