public class EnemyStateDead : EnemyState
{
    public EnemyStateDead(EnemyUnit unit, EnemyStateMachine stateMachine, EnemyMovement unitMovement) 
        : base(unit, stateMachine, unitMovement)
    {
    }

    public override void Update()
    {
    }

    public override void OnEnter()
    {
        _unitMovement.StopMove();
        _unitMovement.enabled = false;
    }
    
    protected override bool CheckTransitionConditions()
    {
        return false;
    }
}
