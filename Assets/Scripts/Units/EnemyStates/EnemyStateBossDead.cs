using UnityEngine;

public class EnemyStateBossDead : EnemyState
{
    private new readonly EnemyMeleeBoss _unit;
    private readonly float _resurrectionDelay;
    private float _timeToResurrection;
    
    public EnemyStateBossDead(EnemyMeleeBoss unit, EnemyStateMachine stateMachine, EnemyMovement unitMovement, float resurrectionDelay) 
        : base(unit, stateMachine, unitMovement)
    {
        _unit = unit;
        _resurrectionDelay = resurrectionDelay;
    }
    
    public override void Update()
    {
        if (_unit.IsFinallyDead)
            return;
        
        if (CheckTransitionConditions())
        {
            _unit.Resurrect();
            _stateMachine.SetState(EnemyStateType.Idle);
            return;
        }

        UpdateResurrectionDelay();
    }
    
    public override void OnEnter()
    {
        _unitMovement.StopMove();
        _unitMovement.enabled = true;
        
        if(!_unit.IsFinallyDead)
            _timeToResurrection = _resurrectionDelay;
    }

    protected override bool CheckTransitionConditions()
    {
        return _timeToResurrection <= 0;
    }
    
    private void UpdateResurrectionDelay()
    {
        _timeToResurrection -= Time.deltaTime;
    }
}
