public class EnemyStateMeleeAttack : EnemyStateAttack
{
    private new readonly EnemyMeleeUnit _unit;

    public EnemyStateMeleeAttack(EnemyMeleeUnit unit, EnemyStateMachine stateMachine, EnemyMovement unitMovement, float attackDelay) 
        : base(unit, stateMachine, unitMovement, attackDelay)
    {
        _unit = unit;
    }
    
    public override void Update()
    {
        if(CheckTransitionConditions())
            return;
        
        UpdateAttackDelay();

        if (CanAttack)
        {
            _unit.Attack();
            _timeToAttack = _attackDelay;
        }
    }
}