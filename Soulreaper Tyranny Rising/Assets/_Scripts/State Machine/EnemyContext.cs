using System.Linq;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]

public class EnemyContext 
{
    public EnemyContext(EntityStatus myStatus, NegativeStatus negativeStatus, NavMeshAgent agent, Animator animator, TargetDetector playerDetector, Transform transform, 
        EnemyAttack enemyAttack, EnemyManager enemyManager)
    {
        this.agent = agent;
        this.animator = animator;
        this.playerDetector = playerDetector;
        this.transform = transform;
        this.enemyAttack = enemyAttack;
        this.myStatus = myStatus;
        this.negativeStatus = negativeStatus;
        this.enemyManager = enemyManager;
    }

    private EntityStatus myStatus;
    private Animator animator;
    private NavMeshAgent agent;
    [SerializeField] private TargetDetector playerDetector;
    private Transform transform;
    private Transform currentTarget;
    private EnemyAttack enemyAttack;
    private NegativeStatus negativeStatus;
    private EnemyManager enemyManager;
    
    public EntityStatus GetMyStatus() => myStatus;

    public Transform GetTransform() => transform;
    
    public NavMeshAgent GetAgent() => agent;
    
    public Animator GetAnimator() => animator;

    public EnemyAttack GetEnemyAttack() => enemyAttack;
    
    public TargetDetector GetTargetDetector() => playerDetector;

    public bool UsingState(EnemyManager.EnemyState state) => enemyManager.myStates.Contains(state);
}
