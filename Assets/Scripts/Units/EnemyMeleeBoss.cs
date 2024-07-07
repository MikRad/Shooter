using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyMeleeBoss : EnemyMeleeUnit, IBossConditionChecker
{
    [SerializeField] private UnitAnimationState[] _attackVariants;
    
    [Header("Life cycle")]
    [SerializeField] private int _lifesNumber = 2;
    [SerializeField] private float _resurrectionDelay = 1.5f;

    private bool _isActivated;

    public bool IsFinallyDead => _lifesNumber == 0;
    
    public event Action OnDied;
    public event Action<float> OnHealthChanged;
    public event Action<bool> OnActivated;

    public void Resurrect()
    {
        _animator.SetTrigger(UnitAnimationIdHelper.GetId(UnitAnimationState.Resurrect));
        
        _health.FillMaxHealth();        
        OnHealthChanged?.Invoke(_health.HealthFullness);

        _bodyCollider.enabled = true;
        _movement.enabled = true;
    }

    public override void HandleDamage(int damageValue)
    {
        base.HandleDamage(damageValue);
        
        OnHealthChanged?.Invoke(_health.HealthFullness);
    }
    
    protected override void InitStateMachine()
    {
        base.InitStateMachine();
        _stateMachine.AddState(EnemyStateType.Pursuit, new EnemyStateBossPursuit(this, _stateMachine, _movement, _player.Transform, SetActivation));
        _stateMachine.AddState(EnemyStateType.RagePursuit, new EnemyStateBossRagePursuit(this, _stateMachine, _movement, _player.Transform, _rageDuration, SetActivation));
        _stateMachine.AddState(EnemyStateType.Dead, new EnemyStateBossDead(this, _stateMachine, _movement, _resurrectionDelay));
    }

    protected override void PlayAttackAnimation()
    {
        if(_attackVariants.Length > 0)
        {
            int randNumber = Random.Range(0, _attackVariants.Length);
            
            _animator.SetTrigger(UnitAnimationIdHelper.GetId(_attackVariants[randNumber]));
        }
        else
        {
            base.PlayAttackAnimation();
        }
    }
    
    protected override void Die()
    {
        base.Die();

        _lifesNumber--;
        SetActivation(false);
        
        if (IsFinallyDead)
        {
            OnDied?.Invoke();
        }
    }

    private void SetActivation(bool activationFlag)
    {
        if (_isActivated != activationFlag)
        {
            _isActivated = activationFlag;
            OnActivated?.Invoke(_isActivated);
        }
    }
}
