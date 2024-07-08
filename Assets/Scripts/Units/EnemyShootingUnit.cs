using UnityEngine;

[RequireComponent(typeof(BaseShooting))]
public class EnemyShootingUnit : EnemyUnit
{
    [Header("Shooting detection")]
    [SerializeField] protected LayerMask _playerShootAccessibilityMask;
    [SerializeField] protected float _additionalPlayerCheckRadius = 0.7f;
    
    private BaseShooting _shooting;

    protected override void Awake()
    {
        base.Awake();
        
        _shooting = GetComponent<BaseShooting>();
    }

    public override void Init(DIContainer diContainer)
    {
        base.Init(diContainer);
        
        BulletSpawner bulletSpawner = diContainer.Resolve<BulletSpawner>();
        _shooting.Init(bulletSpawner, _fxHolder);
        
        // InitStatesAndPositions();
    }
    
    public bool IsPlayerAccessibleToShoot(in Vector3 aimPosition)
    {
        Vector3 direction = aimPosition - _shooting.BulletSpawnPosition;

        RaycastHit2D rayHit = Physics2D.Raycast(_shooting.BulletSpawnPosition, direction, _attackRadius, _playerShootAccessibilityMask);
        
        // return (rayHit.collider != null) && (rayHit.collider.gameObject == _player.gameObject);
        return (rayHit.collider != null) && (rayHit.collider.gameObject.TryGetComponent(out Player _));
    }

    public void UpdateAimPosition(out Vector3 aimPosition)
    {
        aimPosition = _player.Transform.position;
        
        if (IsPlayerAccessibleToShoot(aimPosition))
            return;

        Vector3 additionalCheckPos = aimPosition - _bodyTransform.right * _additionalPlayerCheckRadius;
        
        if (IsPlayerAccessibleToShoot(additionalCheckPos))
        {
            aimPosition += additionalCheckPos;
            return;
        }
        additionalCheckPos = aimPosition + _bodyTransform.right * _additionalPlayerCheckRadius;
        if (IsPlayerAccessibleToShoot(additionalCheckPos))
        {
            aimPosition += additionalCheckPos;
        }
    }

    public override void Deactivate()
    {
        base.Deactivate();

        _shooting.enabled = false;
    }

    protected override void InitStateMachine()
    {
        base.InitStateMachine();
        
        _stateMachine.AddState(EnemyStateType.Attack, new EnemyStateShootingAttack(this, _stateMachine, _movement, _shooting, _attackDelay));
    }
}
