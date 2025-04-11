using UnityEngine;
using UnityEngine.AI;

[System.Serializable]

public class EnemyContext 
{
    public EnemyContext(EntityStatus myStatus, NegativeStatus negativeStatus, NavMeshAgent agent, Animator animator, TargetDetector playerDetector, Transform transform, 
        EnemyAttack enemyAttack)
    {
        _agent = agent;
        _animator = animator;
        _playerDetector = playerDetector;
        _transform = transform;
        _enemyAttack = enemyAttack;
        _myStatus = myStatus;
        _negativeStatus = negativeStatus;
    }

    private EntityStatus _myStatus;
    private Animator _animator;
    private NavMeshAgent _agent;
    [SerializeField] private TargetDetector _playerDetector;
    private Transform _transform;
    private Transform _currentTarget;
    private EnemyAttack _enemyAttack;
    private NegativeStatus _negativeStatus;
    
    public EntityStatus GetMyStatus() => _myStatus;

    public Transform GetTransform() => _transform;
    
    public NavMeshAgent GetAgent() => _agent;
    
    public Animator GetAnimator() => _animator;

    public EnemyAttack GetEnemyAttack() => _enemyAttack;
    
    public TargetDetector GetTargetDetector() => _playerDetector;
}
