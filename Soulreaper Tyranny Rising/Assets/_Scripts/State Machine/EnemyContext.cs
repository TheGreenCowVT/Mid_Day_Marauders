using System.Linq;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]

public class EnemyContext 
{
    public EnemyContext(EntityStatus myStatus, NegativeStatus negativeStatus, NavMeshAgent agent, Animator animator, TargetDetector playerDetector, Transform transform, 
        EnemyAttack enemyAttack, EnemyManager enemyManager)
    {
        _agent = agent;
        _animator = animator;
        _playerDetector = playerDetector;
        _transform = transform;
        _enemyAttack = enemyAttack;
        _myStatus = myStatus;
        _negativeStatus = negativeStatus;
        _enemyManager = enemyManager;
    }

    private EntityStatus _myStatus;
    private Animator _animator;
    private NavMeshAgent _agent;
    [SerializeField] private TargetDetector _playerDetector;
    private Transform _transform;
    private Transform _currentTarget;
    private EnemyAttack _enemyAttack;
    private NegativeStatus _negativeStatus;
    private EnemyManager _enemyManager;
    
    public EntityStatus GetMyStatus() => _myStatus;

    public Transform GetTransform() => _transform;
    
    public NavMeshAgent GetAgent() => _agent;
    
    public Animator GetAnimator() => _animator;

    public EnemyAttack GetEnemyAttack() => _enemyAttack;
    
    public TargetDetector GetTargetDetector() => _playerDetector;

    public bool UsingState(EnemyManager.EnemyState state) => _enemyManager.myStates.Contains(state);
}
