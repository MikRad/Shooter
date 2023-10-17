public class EnemyStateIdle : EnemyState
{
    public EnemyStateIdle(EnemyUnit unit, EnemyStateMachine stateMachine, EnemyMovement unitMovement) 
        : base(unit, stateMachine, unitMovement)
    {
    }

    public override void Update()
    {
        CheckTransitionConditions();
    }

    public override void OnEnter()
    {
        _unitMovement.StopMove();
    }
    
    protected override bool CheckTransitionConditions()
    {
        if (_unit.TryDetectPlayer())
        {
            _stateMachine.SetState(EnemyStateType.Pursuit);
            return true;
        }

        return false;
    }
}
