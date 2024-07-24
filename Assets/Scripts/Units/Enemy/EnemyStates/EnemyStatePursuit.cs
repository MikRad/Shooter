using Units.Abstractions;
using UnityEngine;

namespace Units.Enemy.EnemyStates
{
    public class EnemyStatePursuit : EnemyState
    {
        private readonly Transform _playerTransform;
        private Vector3 _targetPosition;
        
        public EnemyStatePursuit(EnemyUnit unit, EnemyStateMachine stateMachine, EnemyMovement unitMovement, Transform playerTransform) 
            : base(unit, stateMachine, unitMovement)
        {
            _playerTransform = playerTransform;
            _targetPosition = _playerTransform.position;
        }
    
        public override void Update()
        {
            if (CheckTransitionConditions())
                return;
        
            UpdateTargetPosition();
        }
    
        public override void OnEnter()
        {
            UpdateTargetPosition();
            _unitMovement.StartMove();
        }
    
        protected override bool CheckTransitionConditions()
        {
            if (!_unit.TryDetectPlayer())
            {
                _stateMachine.SetState(_unit.IsPatrolRole ? EnemyStateType.Patrol : EnemyStateType.ReturnToStartPosition);
                return true;
            }
            if (_unit.IsPlayerAttackable())
            {
                _stateMachine.SetState(EnemyStateType.Attack);            
                return true;    
            }

            return false;
        }

        protected void UpdateTargetPosition()
        {
            // targetPosition += (playerTransform.position - targetPosition) / targetUpdateDelayFactor;
            _targetPosition = _playerTransform.position;
            _unitMovement.SetTargetPosition(_targetPosition);
        }
    }
}
