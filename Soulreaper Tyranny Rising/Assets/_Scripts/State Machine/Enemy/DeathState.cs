using UnityEngine;

public class DeathState : EnemyBaseState
{
    public DeathState(EnemyContext context, EnemyManager.EnemyState key, ParticleSystem deathParticles) : base(context, key)
    {
        _deathParticles = deathParticles;
    }

    private ParticleSystem _deathParticles;
    private float deactivateTimer;

    public override void EnterState()
    {
        var agent = _context.GetAgent();
        var animator = _context.GetAnimator();
        
        agent.isStopped = true;
        animator.Play("Death");
        _deathParticles.Play();
        deactivateTimer = 3.5f;
    }

    public override void UpdateState()
    {
        deactivateTimer -= Time.deltaTime;
        if(deactivateTimer <= 0)
            _context.GetTransform().gameObject.SetActive(false);
    }

    public override EnemyManager.EnemyState GetNextState()
    {
        return EnemyManager.EnemyState.Death;
    }
}
