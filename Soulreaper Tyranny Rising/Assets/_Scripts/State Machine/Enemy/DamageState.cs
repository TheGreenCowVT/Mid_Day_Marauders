using UnityEngine;

public class DamageState : EnemyBaseState
{
    public DamageState(EnemyContext context, EnemyManager.EnemyState key) : base(context, key)
    {
        
    }

    private bool _reactionOver = false;

    public override void EnterState()
    {
        var status = _context.GetMyStatus();
        var animator = _context.GetAnimator();
        var agent = _context.GetAgent();

        agent.isStopped = true;
        agent.Warp(_context.GetTransform().position);
        agent.updateRotation = false;

        if (status.HeavyFlinch())
        {
            switch (status.GetRelativePosition(status.GetDamagePosition()))
            {
                case "Front":
                    animator.CrossFade("Strong Front Damage", 0.02f);
                    break;
                case "Back":
                    animator.CrossFade("Strong Back Damage", 0.02f);
                    break;
                case "Left":
                    animator.CrossFade("Strong Left Damage", 0.02f);
                    break;
                case "Right":
                    animator.CrossFade("Strong Right Damage", 0.02f);
                    break;
                default:
                    animator.CrossFade("Strong Front Damage", 0.02f);
                    break;
            }
        }
        
        else if (status.Flinch())
        {
            switch (status.GetRelativePosition(status.GetDamagePosition()))
            {
                case "Front":
                    animator.CrossFade("Front Damage", 0.02f);
                    break;
                case "Back":
                    animator.CrossFade("Back Damage", 0.02f);
                    break;
                case "Left":
                    animator.CrossFade("Left Damage", 0.02f);
                    break;
                case "Right":
                    animator.CrossFade("Right Damage", 0.02f);
                    break;
                default:
                    animator.CrossFade("Front Damage", 0.02f);
                    break;
            }
        }
        
        status.HandleReaction();
        _reactionOver = false;
    }

    public override void UpdateState()
    {
        var status = _context.GetMyStatus();
        var animator = _context.GetAnimator();
        
        if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f) _reactionOver = true;
        
        if (status.HeavyFlinch())
        {
            switch (status.GetRelativePosition(status.GetDamagePosition()))
            {
                case "Front":
                    animator.CrossFade("Strong Front Damage", 0.02f);
                    break;
                case "Back":
                    animator.CrossFade("Strong Back Damage", 0.02f);
                    break;
                case "Left":
                    animator.CrossFade("Strong Left Damage", 0.02f);
                    break;
                case "Right":
                    animator.CrossFade("Strong Right Damage", 0.02f);
                    break;
                default:
                    animator.CrossFade("Strong Front Damage", 0.02f);
                    break;
            }
        }
        
        else if (status.Flinch())
        {
            switch (status.GetRelativePosition(status.GetDamagePosition()))
            {
                case "Front":
                    animator.CrossFade("Front Damage", 0.02f);
                    break;
                case "Back":
                    animator.CrossFade("Back Damage", 0.02f);
                    break;
                case "Left":
                    animator.CrossFade("Left Damage", 0.02f);
                    break;
                case "Right":
                    animator.CrossFade("Right Damage", 0.02f);
                    break;
                default:
                    animator.CrossFade("Front Damage", 0.02f);
                    break;
            }
        }
        
        status.HandleReaction();
    }

    public override EnemyManager.EnemyState GetNextState()
    {
        var status = _context.GetMyStatus();
        
        if (!status.IsAlive()) return EnemyManager.EnemyState.Death;
        if (_reactionOver) return EnemyManager.EnemyState.Idle;

        return EnemyManager.EnemyState.Damage;
    }
}
