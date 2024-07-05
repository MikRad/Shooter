using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(EnemyMovement))]
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
    
    [Header("Movement")]
    [SerializeField] protected Transform _patrolPointsTransform;
    
    [Header("Item generation")]
    [Range(0, 100)]
    [SerializeField] private int _itemGenerationProbability;
    [SerializeField] private PickupGenerationInfo[] _pickupGenerationInfos;

    protected EnemyMovement _movement;
    private Vector3 _startPosition;
    
    protected EnemyStateMachine _stateMachine;
    private readonly List<Vector3> _patrolPoints = new List<Vector3>();
    
    protected Player _player;
    protected PickupItemSpawner _pickupItemSpawner;
    
    public bool IsPatrolRole { get; private set;}
    
    public static event Action<EnemyUnit> OnCreated;
    
    protected override void Awake()
    {
        base.Awake();
        
        _movement = GetComponent<EnemyMovement>();
    }

    protected override void Start()
    {
        base.Start();
        
        OnCreated?.Invoke(this);
    }
    
    protected virtual void Update()
    {
        _stateMachine.Update();
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

    protected void InitStatesAndPositions()
    {
        for (int i = 0; i < _patrolPointsTransform.childCount; i++)
        {
            _patrolPoints.Add(_patrolPointsTransform.GetChild(i).position);
        }
        Destroy(_patrolPointsTransform.gameObject);
        
        _startPosition = _cachedTransform.position;
        IsPatrolRole = (_patrolPoints.Count >= 2);
        
        InitStateMachine();
    }
    
    protected virtual void InitStateMachine()
    {
        _stateMachine = new EnemyStateMachine();
        _stateMachine.AddState(EnemyStateType.Idle, new EnemyStateIdle(this, _stateMachine, _movement));
        _stateMachine.AddState(EnemyStateType.Patrol, new EnemyStatePatrol(this, _stateMachine, _movement, _patrolPoints));
        _stateMachine.AddState(EnemyStateType.ReturnToStartPosition, new EnemyStateReturn(this, _stateMachine, _movement, _startPosition));
        _stateMachine.AddState(EnemyStateType.Pursuit, new EnemyStatePursuit(this, _stateMachine, _movement, _player.Transform));
        _stateMachine.AddState(EnemyStateType.RagePursuit, new EnemyStateRagePursuit(this, _stateMachine, _movement, _player.Transform, _rageDuration));
        _stateMachine.AddState(EnemyStateType.Dead, new EnemyStateDead(this, _stateMachine, _movement));
        
        _stateMachine.SetState(IsPatrolRole ? EnemyStateType.Patrol : EnemyStateType.Idle);
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
        TryGenerateItem();
    }

    private void TryGenerateItem()
    {
        if ((_pickupGenerationInfos == null) || (_pickupGenerationInfos.Length == 0))
            return;

        int randProbability = Random.Range(1, 101);
        if(randProbability <= _itemGenerationProbability)
        {
            int totalWeight = 0;
            foreach (PickupGenerationInfo info in _pickupGenerationInfos)
            {
                totalWeight += info.probabilityWeight;
            }

            int randWeight = Random.Range(0, totalWeight + 1);
            int cumulativeWeight = 0;
            for (int i = 0; i < _pickupGenerationInfos.Length; i++)
            {
                cumulativeWeight += _pickupGenerationInfos[i].probabilityWeight;
                if (cumulativeWeight >= randWeight)
                {
                    _pickupItemSpawner.SpawnItem(_pickupGenerationInfos[i].itemType, _cachedTransform.position);
                    return;
                }
            }
        }
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
            for (int i = 0; i < _patrolPoints.Count; i++)
            {
                if ((i + 1) < _patrolPoints.Count)
                    Gizmos.DrawLine(_patrolPoints[i], _patrolPoints[i + 1]);
                else
                    Gizmos.DrawLine(_patrolPoints[i], _patrolPoints[0]);
            }
        }
        else
        {
            Gizmos.color = Color.magenta;
            for (int i = 0; i < _patrolPointsTransform.childCount; i++)
            {
                if ((i + 1) < _patrolPointsTransform.childCount)
                    Gizmos.DrawLine(_patrolPointsTransform.GetChild(i).position, _patrolPointsTransform.GetChild(i + 1).position);
                else
                    Gizmos.DrawLine(_patrolPointsTransform.GetChild(i).position, _patrolPointsTransform.GetChild(0).position);
            }
        }
#endif
    }
}
