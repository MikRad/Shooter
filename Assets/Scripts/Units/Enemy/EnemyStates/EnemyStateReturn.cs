using Units.Abstractions;
using UnityEngine;

namespace Units.Enemy.EnemyStates
{
    public class EnemyStateReturn : EnemyState
    {
        private readonly Vector3 _startPosition;
        private const float DistanceCompareDelta = 0.01f;

        public EnemyStateReturn(EnemyUnit unit, EnemyStateMachine stateMachine, EnemyMovement enemyMovement, Vector3 startPos) 
            : base(unit, stateMachine, enemyMovement)
        {
            _startPosition = startPos;
        }

        public override void Update()
        {
            CheckTransitionConditions();
        }

        public override void OnEnter()
        {
            _unitMovement.StartMove(EnemyMoveType.Return);
            _unitMovement.SetTargetPosition(_startPosition);
        }
    
        protected override bool CheckTransitionConditions()
        {
            if (_unit.TryDetectPlayer())
            {
                _stateMachine.SetState(EnemyStateType.Pursuit);
                return true;
            }
        
            if (Vector3.SqrMagnitude(_unitTransform.position - _startPosition) <= DistanceCompareDelta)
            {
                _stateMachine.SetState(EnemyStateType.Idle);
                return true;
            }

            return false;
        }
    }
}
