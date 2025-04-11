using UnityEngine;

public class AttackState : EnemyBaseState
{
    public AttackState(EnemyContext context, EnemyManager.EnemyState key) : base(context, key)
    {
       
    }
    
    private readonly int StabHash = Animator.StringToHash("Stab");
    private readonly int QuickStabHash = Animator.StringToHash("Quick Stab");
    private readonly int JumpAttackHash = Animator.StringToHash("Jump Attack");
    private readonly int Combo1Hash = Animator.StringToHash("Combo 1");
    private bool attackOver;
    
    public override void EnterState()
    {
        //Debug.Log("ATTACK STATE");
        var agent = context.GetAgent();
        var animator = context.GetAnimator();
        var myStatus = context.GetMyStatus();
        var transform = context.GetTransform();
        
        agent.updateRotation = false;
        agent.Warp(transform.position);
        animator.SetFloat("forward", 0);
        
        if (myStatus.CurrentLifePoints > myStatus.GetMaxLP() * 0.5f)
        {
            var rand = Random.Range(0, 2);
            switch (rand)
            {
                case 0:
                    animator.CrossFade(QuickStabHash, 0.2f, 0, 0f);
                    break;
                case 1:
                    animator.CrossFade(StabHash, 0.2f, 0, 0f);
                    break;
                case 2:
                    animator.CrossFade(JumpAttackHash, 0.2f, 0, 0f);
                    break;
            }
        }
        else
        {
            animator.CrossFade(Combo1Hash, 0.2f, 0, 0f);
        }

        attackOver = false;
    }

    public override void UpdateState()
    {
        var attackController = context.GetEnemyAttack();
        var animator = context.GetAnimator();
        var target = context.GetTargetDetector().GetTarget();
        var transform = context.GetTransform();

        if (!attackController.GetRotationLocked())
        {
            // Get direction to target (ignoring Y-axis difference)
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

        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            attackOver = true;
        }
    }

    public override EnemyManager.EnemyState GetNextState()
    {
        var status = context.GetMyStatus();
        var useSpacing = context.UsingState(EnemyManager.EnemyState.Spacing);
        var focusIdle = context.UsingState(EnemyManager.EnemyState.FocusIdle);

        if (!status.IsAlive()) return EnemyManager.EnemyState.Death;

        if (status.HeavyFlinch()) return EnemyManager.EnemyState.Damage;
        else if (status.Flinch()) return EnemyManager.EnemyState.Damage;

        if (attackOver && useSpacing) return EnemyManager.EnemyState.Spacing;
        if (attackOver && !useSpacing)
        {
            if (focusIdle) return EnemyManager.EnemyState.FocusIdle;
            else return EnemyManager.EnemyState.Idle;
        }

        return EnemyManager.EnemyState.Attack;
    }
}
