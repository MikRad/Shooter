using System.Collections.Generic;
using Units.Abstractions;
using UnityEngine;

namespace Units.Enemy.EnemyStates
{
    public class EnemyStatePatrol : EnemyState
    {
        private readonly IReadOnlyList<Vector3> _patrolPoints;
        private int _currentPatrolPointIndex;
        private const float DistanceCompareDelta = 0.01f;

        public EnemyStatePatrol(EnemyUnit unit, EnemyStateMachine stateMachine, EnemyMovement unitMovement, IReadOnlyList<Vector3> patrolPoints) 
            : base(unit, stateMachine, unitMovement)
        {
            _patrolPoints = patrolPoints;
        }

        public override void Update()
        {
            if (CheckTransitionConditions())
                return;
        
            if (Vector3.SqrMagnitude(_unitTransform.position - _patrolPoints[_currentPatrolPointIndex]) <= DistanceCompareDelta)
            {
                _currentPatrolPointIndex++;
                if (_currentPatrolPointIndex == _patrolPoints.Count)
                    _currentPatrolPointIndex = 0;

                _unitMovement.SetTargetPosition(_patrolPoints[_currentPatrolPointIndex]);
            }
        }
    
        public override void OnEnter()
        {
            _unitMovement.StartMove(EnemyMoveType.Patrol);
            _unitMovement.SetTargetPosition(_patrolPoints[_currentPatrolPointIndex]);
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
}
