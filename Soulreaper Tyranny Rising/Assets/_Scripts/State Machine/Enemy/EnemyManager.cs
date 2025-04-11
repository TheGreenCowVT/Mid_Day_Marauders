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
        AgentChase,
        Attack,
        Defend,
        Spacing,
        Damage,
        Death
    }

    private EntityStatus myStatus;
    private NegativeStatus negativeStatus;
    private NavMeshAgent agent;
    private Animator animator;
    private TargetDetector playerDetector;
    private EnemyAttack enemyAttack;
    [SerializeField] private EnemyContext context;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float delay = 0.25f;
    private Vector2 moveVelocity;
    private Vector2 smoothDeltaPosition;
    public ParticleSystem deathParticles;
    public EnemyState[] myStates;
    public bool useRootMotion;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        playerDetector = GetComponentInChildren<TargetDetector>();
        myStatus = GetComponent<EntityStatus>();
        negativeStatus = GetComponent<NegativeStatus>();
        enemyAttack = GetComponent<EnemyAttack>();
    }

    void InitializeStates()
    {
        states = new Dictionary<EnemyState, BaseState<EnemyState>>();
        
        states.Add(EnemyState.Idle, new IdleState(context, EnemyState.Idle));
        states.Add(EnemyState.FocusIdle, new IdleState(context, EnemyState.Idle));
        states.Add(EnemyState.Damage, new DamageState(context, EnemyState.Damage));
        states.Add(EnemyState.Death, new DeathState(context, EnemyState.Death,
            deathParticles));

        if (myStates.Contains(EnemyState.Chase)) states.Add(EnemyState.Chase, new ChaseState(context, EnemyState.Chase, attackRange, delay));

        if (myStates.Contains(EnemyState.AgentChase)) states.Add(EnemyState.AgentChase, new AgentChaseState(context, EnemyState.Chase, attackRange, moveSpeed, delay));

        if (myStates.Contains(EnemyState.Attack)) states.Add(EnemyState.Attack, new AttackState(context, EnemyState.Attack));

        if(myStates.Contains(EnemyState.Spacing)) states.Add(EnemyState.Spacing, new SpacingState(context, EnemyState.Spacing, attackRange));
    }

    void Start()
    {
        context = new EnemyContext(myStatus, negativeStatus, agent, animator, playerDetector, transform, enemyAttack, this);
        InitializeStates();
    }

    private void OnEnable()
    {
           Invoke("DelayActivate", 1f);
    }

    void DelayActivate()
    {
        currentState = states[EnemyState.Idle];
        isInitialized = true;
    }

    void OnAnimatorMove()
    {
       Vector3 rootPosition = animator.rootPosition;
       rootPosition.y = agent.nextPosition.y;
       transform.position = rootPosition;
       agent.nextPosition = rootPosition;
    }

    public override void Update()
    {
        base.Update();
        if(useRootMotion) SynchronizeAnimatorAndAgent();
    }

    private void SynchronizeAnimatorAndAgent()
    {
        if(!myStatus.IsAlive() || !agent.isOnNavMesh) return;
        
        
        var worldDeltaPosition = agent.nextPosition - transform.position;
        worldDeltaPosition.y = 0;
        
        float dx = Vector3.Dot(transform.right, worldDeltaPosition);
        float dy = Vector3.Dot(transform.forward, worldDeltaPosition);
        var deltaPosition = new Vector2(dx, dy);

        var smooth = Mathf.Min(1, Time.deltaTime / 0.1f);
        smoothDeltaPosition = Vector2.Lerp(smoothDeltaPosition, deltaPosition, smooth);
        
        moveVelocity = smoothDeltaPosition / Time.deltaTime;

        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            moveVelocity = Vector2.Lerp(
                Vector2.zero,
                moveVelocity,
                agent.remainingDistance / agent.stoppingDistance);
        }

        if (agent.remainingDistance > agent.stoppingDistance)
        {
            animator.SetFloat("forward", moveVelocity.magnitude);
        }
        else
        {
            animator.SetFloat("forward", 0);
        }

        var deltaMagnitude = worldDeltaPosition.magnitude;
        if (deltaMagnitude > agent.radius )
        {
            transform.position = Vector3.Lerp(
                animator.rootPosition,
                agent.nextPosition,
                smooth);
        }
    }
}
