using UnityEngine;

public class EnemyMeleeUnit : EnemyUnit
{
    [Header("Attack")]
    [SerializeField] protected int _damageAmount = 2;
    
    private MeleeAnimationHandler _animationHandler;
    
    protected override void Awake()
    {
        base.Awake();
        
        _animationHandler = GetComponentInChildren<MeleeAnimationHandler>();
    }

    protected void OnEnable()
    {
        _animationHandler.OnAttackPerformed += DoDamage;
    }

    protected void OnDisable()
    {
        _animationHandler.OnAttackPerformed -= DoDamage;
    }

    // public override void Init(DIContainer diContainer)
    // {
    //     base.Init(diContainer);
    //
    //     InitStatesAndPositions();
    // }
    
    public virtual void Attack()
    {
        PlayAttackAnimation();
    }
    
    protected override void InitStateMachine()
    {
        base.InitStateMachine();
        _stateMachine.AddState(EnemyStateType.Attack, new EnemyStateMeleeAttack(this, _stateMachine, _movement, _attackDelay));
    }

    private void DoDamage()
    {
        _player.HandleDamage(_damageAmount);
    }

    protected virtual void PlayAttackAnimation()
    {
        _animator.SetTrigger(UnitAnimationIdHelper.GetId(UnitAnimationState.Attack));
    }
}
