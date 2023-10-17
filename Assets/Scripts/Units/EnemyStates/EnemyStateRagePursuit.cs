using UnityEngine;

public class EnemyStateRagePursuit : EnemyStatePursuit
{
    private readonly float _rageDuration;
    private float _currentRageTime;

    private bool IsRageTimeExpired => _currentRageTime <= 0;
    
    public EnemyStateRagePursuit(EnemyUnit unit, EnemyStateMachine stateMachine, EnemyMovement unitMovement, Transform playerTransform, float duration) 
        : base(unit, stateMachine, unitMovement, playerTransform)
    {
        _rageDuration = duration;
    }

    public override void Update()
    {
        UpdateRageTime();

        base.Update();
    }
    
    public override void OnEnter()
    {
        _currentRageTime = _rageDuration;
        
        UpdateTargetPosition();
        _unitMovement.StartMove(EnemyMoveType.Rage);
    }
    
    protected override bool CheckTransitionConditions()
    {
        if (IsRageTimeExpired)
        {
            _stateMachine.SetState(EnemyStateType.Pursuit);            
            return true;
        }
        if (_unit.IsPlayerAttackable())
        {
            _stateMachine.SetState(EnemyStateType.Attack);            
            return true;    
        }

        return false;
    }
    
    private void UpdateRageTime()
    {
        _currentRageTime -= Time.deltaTime;
    }
}
