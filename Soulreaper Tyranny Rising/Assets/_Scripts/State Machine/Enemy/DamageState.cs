using UnityEngine;

public class DamageState : EnemyBaseState
{
    public DamageState(EnemyContext context, EnemyManager.EnemyState key) : base(context, key)
    {
        
    }

    private bool reactionOver = false;

    public override void EnterState()
    {
        var status = context.GetMyStatus();
        var animator = context.GetAnimator();
        var agent = context.GetAgent();

        agent.isStopped = true;
        agent.Warp(context.GetTransform().position);
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
        reactionOver = false;
    }

    public override void UpdateState()
    {
        var status = context.GetMyStatus();
        var animator = context.GetAnimator();
        
        if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f) reactionOver = true;
        
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
        var status = context.GetMyStatus();
        
        if (!status.IsAlive()) return EnemyManager.EnemyState.Death;
        if (reactionOver) return EnemyManager.EnemyState.Idle;

        return EnemyManager.EnemyState.Damage;
    }
}
