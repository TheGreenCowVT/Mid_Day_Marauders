using UnityEngine;

public class DeathState : EnemyBaseState
{
    public DeathState(EnemyContext context, EnemyManager.EnemyState key, ParticleSystem deathParticles) : base(context, key)
    {
        this.deathParticles = deathParticles;
    }

    private ParticleSystem deathParticles;
    private float deactivateTimer;

    public override void EnterState()
    {
        var agent = context.GetAgent();
        var animator = context.GetAnimator();
        
        agent.isStopped = true;
        animator.Play("Death");
        deathParticles.Play();
        deactivateTimer = 3.5f;
    }

    public override void UpdateState()
    {
        deactivateTimer -= Time.deltaTime;
        if(deactivateTimer <= 0)
            context.GetTransform().gameObject.SetActive(false);
    }

    public override EnemyManager.EnemyState GetNextState()
    {
        return EnemyManager.EnemyState.Death;
    }
}
