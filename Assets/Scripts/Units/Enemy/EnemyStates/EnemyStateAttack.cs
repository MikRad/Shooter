using Units.Abstractions;
using UnityEngine;

namespace Units.Enemy.EnemyStates
{
    public abstract class EnemyStateAttack : EnemyState
    {
        protected readonly float _attackDelay;
        protected float _timeToAttack;

        protected bool CanAttack => _timeToAttack <= 0;

        protected EnemyStateAttack(EnemyUnit unit, EnemyStateMachine stateMachine, EnemyMovement unitMovement, float attackDelay) 
            : base(unit, stateMachine, unitMovement)
        {
            _attackDelay = attackDelay;
        }

        public override void OnEnter()
        {
            _timeToAttack = 0;
            _unitMovement.StopMove();
        }
    
        protected void UpdateAttackDelay()
        {
            if (_timeToAttack > 0)
            {
                _timeToAttack -= Time.deltaTime;
            }
        }
    
        protected override bool CheckTransitionConditions()
        {
            if (!_unit.IsPlayerAttackable())
            {
                _stateMachine.SetState(EnemyStateType.Pursuit);
                return true;
            }
        
            return false;
        }
    }
}