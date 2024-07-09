using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyMovement))]
[RequireComponent(typeof(PickupItemGenerator))]
public abstract class EnemyUnit : BaseUnit
{
    [Header("Vision")]
    [Range(30, 360)]
    [SerializeField] protected float _visionAngle;
    [SerializeField] protected LayerMask _obstaclesMask;
    
    [Header("Base settings")]
    [SerializeField] protected float _pursuitRadius = 16f;
    [SerializeField] protected float _targetDetectionVisionRadius = 16f;
    [SerializeField] protected float _targetDetectionAnywayRadius = 12f;
    [SerializeField] protected float _attackRadius = 10f;
    [SerializeField] protected float _rageDuration = 5f;
    
    protected EnemyMovement _movement;
    private Vector3 _startPosition;
    
    protected EnemyStateMachine _stateMachine;
    private List<Vector3> _patrolPositions = new List<Vector3>();
    
    protected Player _player;
    protected PickupItemGenerator _pickupItemGenerator;
    
    public bool IsPatrolRole { get; private set;}
    
    protected override void Awake()
    {
        base.Awake();
        
        _movement = GetComponent<EnemyMovement>();
        _pickupItemGenerator = GetComponent<PickupItemGenerator>();
        
        EventBus.Get.Subscribe<PlayerCreatedEvent>(HandlePlayerCreated);
    }

    protected override void Start()
    {
        base.Start();

        EventBus.Get.RaiseEvent(this, new EnemyCreatedEvent(this));
    }
    
    protected virtual void Update()
    {
        _stateMachine.Update();
    }

    public virtual void Init(DIContainer diContainer)
    {
        _pickupItemGenerator.Init(diContainer.Resolve<PickupItemSpawner>());
    }

    private void OnDestroy()
    {
        EventBus.Get.Unsubscribe<PlayerCreatedEvent>(HandlePlayerCreated);
    }

    public bool TryDetectPlayer()
    {
        if (_player.IsDead)
            return false;
        
        float distanceToPlayer = Vector3.Distance(_player.Transform.position, _cachedTransform.position);

        if (distanceToPlayer < _targetDetectionAnywayRadius)
            return true;
        
        return (distanceToPlayer < _targetDetectionVisionRadius) && (IsPlayerVisible());
    }
    
    public bool IsPlayerAttackable()
    {
        if (_player.IsDead)
            return false;
        
        float distanceToPlayer = Vector3.Distance(_player.Transform.position, _cachedTransform.position);

        return distanceToPlayer < _attackRadius;
    }

    public override void Deactivate()
    {
        base.Deactivate();
        
        _movement.StopMove();
        _movement.enabled = false;
    }

    public override void HandleDamage(int damageAmount)
    {
        _stateMachine.SetState(EnemyStateType.RagePursuit);
        
        base.HandleDamage(damageAmount);
    }

    public void SetPatrolPositions(List<Vector3> patrolPositions)
    {
        _patrolPositions = patrolPositions;
    }
    
    protected void InitStates()
    {
        // for (int i = 0; i < _patrolPointsTransform.childCount; i++)
        // {
        //     _patrolPoints.Add(_patrolPointsTransform.GetChild(i).position);
        // }
        // Destroy(_patrolPointsTransform.gameObject);
        
        _startPosition = _cachedTransform.position;
        IsPatrolRole = (_patrolPositions.Count >= 2);
        
        InitStateMachine();
    }
    
    protected virtual void InitStateMachine()
    {
        _stateMachine = new EnemyStateMachine();
        _stateMachine.AddState(EnemyStateType.Idle, new EnemyStateIdle(this, _stateMachine, _movement));
        _stateMachine.AddState(EnemyStateType.Patrol, new EnemyStatePatrol(this, _stateMachine, _movement, _patrolPositions));
        _stateMachine.AddState(EnemyStateType.ReturnToStartPosition, new EnemyStateReturn(this, _stateMachine, _movement, _startPosition));
        _stateMachine.AddState(EnemyStateType.Pursuit, new EnemyStatePursuit(this, _stateMachine, _movement, _player.Transform));
        _stateMachine.AddState(EnemyStateType.RagePursuit, new EnemyStateRagePursuit(this, _stateMachine, _movement, _player.Transform, _rageDuration));
        _stateMachine.AddState(EnemyStateType.Dead, new EnemyStateDead(this, _stateMachine, _movement));
        
        _stateMachine.SetState(IsPatrolRole ? EnemyStateType.Patrol : EnemyStateType.Idle);
    }
    
    private void HandlePlayerCreated(PlayerCreatedEvent ev)
    {
        _player = ev.Player;
        
        InitStates();
    }
    
    private bool IsPlayerVisible()
    {
        Vector3 bodyDir = _bodyTransform.up;
        Vector3 dirToPlayer = _player.Transform.position - _cachedTransform.position;
        float angleToPlayer = Vector3.Angle(dirToPlayer, bodyDir);

        if (angleToPlayer > (_visionAngle / 2))
            return false;

        RaycastHit2D rHit = Physics2D.Raycast(_cachedTransform.position, dirToPlayer, _targetDetectionVisionRadius, _obstaclesMask);
        
        return rHit.collider == null;
    }
    
    protected override void Die()
    {
        _stateMachine.SetState(EnemyStateType.Dead);
        
        base.Die();
        
        _pickupItemGenerator.TryGenerateItem(_cachedTransform.position);
    }

    
    private void OnDrawGizmos()
    {
        if (UnityEditor.EditorApplication.isPlaying && IsDead)
            return;

        Vector3 position = transform.position;
        
        Gizmos.color = new Color(0.25f, 1f, 0.0f, 0.2f);
        Gizmos.DrawWireSphere(position, _targetDetectionVisionRadius);
        Gizmos.DrawWireSphere(position, _targetDetectionAnywayRadius);
        Gizmos.DrawWireSphere(position, _pursuitRadius);
        Gizmos.DrawWireSphere(position, _attackRadius);

        Gizmos.color = new Color(1f, 0f, 0f, 0.75f);
        Quaternion rotLeft = Quaternion.AngleAxis(-_visionAngle / 2, _bodyTransform.forward);
        Quaternion rotRight = Quaternion.AngleAxis(_visionAngle / 2, _bodyTransform.forward);
        Vector3 rayLeft = rotLeft * _bodyTransform.up;
        Vector3 rayRight = rotRight * _bodyTransform.up;

        Gizmos.DrawRay(position, rayLeft * _targetDetectionVisionRadius);
        Gizmos.DrawRay(position, _bodyTransform.up * _attackRadius);
        Gizmos.DrawRay(position, rayRight * _targetDetectionVisionRadius);

#if UNITY_EDITOR

        if (UnityEditor.EditorApplication.isPlaying)
        {
            Gizmos.color = Color.magenta;
            for (int i = 0; i < _patrolPositions.Count; i++)
            {
                if ((i + 1) < _patrolPositions.Count)
                    Gizmos.DrawLine(_patrolPositions[i], _patrolPositions[i + 1]);
                else
                    Gizmos.DrawLine(_patrolPositions[i], _patrolPositions[0]);
            }
        }
        // else
        // {
        //     Gizmos.color = Color.magenta;
        //     for (int i = 0; i < _patrolPointsTransform.childCount; i++)
        //     {
        //         if ((i + 1) < _patrolPointsTransform.childCount)
        //             Gizmos.DrawLine(_patrolPointsTransform.GetChild(i).position, _patrolPointsTransform.GetChild(i + 1).position);
        //         else
        //             Gizmos.DrawLine(_patrolPointsTransform.GetChild(i).position, _patrolPointsTransform.GetChild(0).position);
        //     }
        // }
#endif
    }
}
