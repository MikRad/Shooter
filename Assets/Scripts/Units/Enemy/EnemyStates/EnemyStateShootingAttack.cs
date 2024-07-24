using UnityEngine;

namespace Units.Enemy.EnemyStates
{
    public class EnemyStateShootingAttack : EnemyStateAttack
    {
        private new readonly EnemyShootingUnit _unit;
        private readonly BaseShooting _unitShooting;
        private Vector3 _aimPosition;
    
        public EnemyStateShootingAttack(EnemyShootingUnit unit, EnemyStateMachine stateMachine, EnemyMovement movement, 
            BaseShooting shooting, float attackDelay) : base(unit, stateMachine, movement, attackDelay)
        {
            _unit = unit;
            _unitShooting = shooting;
        }
    
        public override void Update()
        {
            if(CheckTransitionConditions())
                return;
        
            UpdateAttackDelay();
            _unit.UpdateAimPosition(out _aimPosition);
            _unitMovement.SetTargetPosition(_aimPosition);

            if (CanAttack && _unit.IsPlayerAccessibleToShoot(_aimPosition))
            {
                _unitShooting.ShootAt(_aimPosition);
                _timeToAttack = _attackDelay;
            }
        }

        public override void OnEnter()
        {
            _unitMovement.StopMove();
        }
    }
}
