using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyManager : StateManager<EnemyManager.EnemyState>
{
    public enum EnemyState
    {
        Idle,
        FocusIdle,
        Chase,
        Attack,
        Defend,
        Spacing,
        Damage,
        Death
    }

    private EntityStatus _myStatus;
    private NegativeStatus _negativeStatus;
    private NavMeshAgent _agent;
    private Animator _animator;
    private TargetDetector _playerDetector;
    private EnemyAttack _enemyAttack;
    [SerializeField] private EnemyContext _context;
    [SerializeField] private float _attackRange = 1.5f;
    private Vector2 _moveVelocity;
    private Vector2 _smoothDeltaPosition;
    public ParticleSystem _deathParticles;
    public EnemyState[] myStates;

    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _playerDetector = GetComponentInChildren<TargetDetector>();
        _myStatus = GetComponent<EntityStatus>();
        _negativeStatus = GetComponent<NegativeStatus>();
        _enemyAttack = GetComponent<EnemyAttack>();
    }

    void InitializeStates()
    {
        _states = new Dictionary<EnemyState, BaseState<EnemyState>>();
        
        _states.Add(EnemyState.Idle, new IdleState(_context, EnemyState.Idle));
        _states.Add(EnemyState.FocusIdle, new IdleState(_context, EnemyState.Idle));
        _states.Add(EnemyState.Damage, new DamageState(_context, EnemyState.Damage));
        _states.Add(EnemyState.Death, new DeathState(_context, EnemyState.Death,
            _deathParticles));

        if (myStates.Contains(EnemyState.Chase)) _states.Add(EnemyState.Chase, new ChaseState(_context, EnemyState.Chase, _attackRange));

        if (myStates.Contains(EnemyState.Attack)) _states.Add(EnemyState.Attack, new AttackState(_context, EnemyState.Attack));

        if(myStates.Contains(EnemyState.Spacing)) _states.Add(EnemyState.Spacing, new SpacingState(_context, EnemyState.Spacing, _attackRange));
    }

    void Start()
    {
        _context = new EnemyContext(_myStatus, _negativeStatus, _agent, _animator, _playerDetector, transform, _enemyAttack, this);
        InitializeStates();
    }

    private void OnEnable()
    {
           Invoke("DelayActivate", 1f);
    }

    void DelayActivate()
    {
        _currentState = _states[EnemyState.Idle];
        _isInitialized = true;
    }

    void OnAnimatorMove()
    {
       Vector3 rootPosition = _animator.rootPosition;
       rootPosition.y = _agent.nextPosition.y;
       transform.position = rootPosition;
       _agent.nextPosition = rootPosition;
    }

    public override void Update()
    {
        base.Update();
        SynchronizeAnimatorAndAgent();
    }

    private void SynchronizeAnimatorAndAgent()
    {
        if(!_myStatus.IsAlive() || !_agent.isOnNavMesh) return;
        
        
        var worldDeltaPosition = _agent.nextPosition - transform.position;
        worldDeltaPosition.y = 0;
        
        float dx = Vector3.Dot(transform.right, worldDeltaPosition);
        float dy = Vector3.Dot(transform.forward, worldDeltaPosition);
        var deltaPosition = new Vector2(dx, dy);

        var smooth = Mathf.Min(1, Time.deltaTime / 0.1f);
        _smoothDeltaPosition = Vector2.Lerp(_smoothDeltaPosition, deltaPosition, smooth);
        
        _moveVelocity = _smoothDeltaPosition / Time.deltaTime;

        if (_agent.remainingDistance <= _agent.stoppingDistance)
        {
            _moveVelocity = Vector2.Lerp(
                Vector2.zero,
                _moveVelocity,
                _agent.remainingDistance / _agent.stoppingDistance);
        }

        if (_agent.remainingDistance > _agent.stoppingDistance)
        {
            _animator.SetFloat("forward", _moveVelocity.magnitude);
        }
        else
        {
            _animator.SetFloat("forward", 0);
        }

        var deltaMagnitude = worldDeltaPosition.magnitude;
        if (deltaMagnitude > _agent.radius )
        {
            transform.position = Vector3.Lerp(
                _animator.rootPosition,
                _agent.nextPosition,
                smooth);
        }
    }
}
