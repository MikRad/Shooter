using System;
using Units.Abstractions;
using UnityEngine;

namespace Units.Enemy.EnemyStates
{
    public class EnemyStateBossPursuit : EnemyStatePursuit
    {
        private const float PursuitStoppingDelay = 2f;
        private bool _isGoingToStopPursuit;
        private float _timeToStopPursuit;
    
        private readonly Action<bool> _activationCallback;

        public EnemyStateBossPursuit(EnemyUnit unit, EnemyStateMachine stateMachine, EnemyMovement unitMovement, Transform playerTransform, Action<bool> activationCheckCallback) 
            : base(unit, stateMachine, unitMovement, playerTransform)
        {
            _activationCallback = activationCheckCallback;
        }

        public override void OnEnter()
        {
            base.OnEnter();
        
            _activationCallback(true);
            _isGoingToStopPursuit = false;
        }
    
        private bool UpdateStoppingPursuit()
        {
            _timeToStopPursuit -= Time.deltaTime;
            if (_timeToStopPursuit < 0)
            {
                _stateMachine.SetState(_unit.IsPatrolRole ? EnemyStateType.Patrol : EnemyStateType.ReturnToStartPosition);
                _activationCallback(false);
                return true;
            }

            return false;
        }
    
        protected override bool CheckTransitionConditions()
        {
            if (_unit.TryDetectPlayer())
            {
                _isGoingToStopPursuit = false;
            }
            else
            {
                if (!_isGoingToStopPursuit)
                {
                    _isGoingToStopPursuit = true;
                    _timeToStopPursuit = PursuitStoppingDelay;
                }
            }

            if (_isGoingToStopPursuit)
            {
                return UpdateStoppingPursuit();
            }
            if (_unit.IsPlayerAttackable())
            {
                _stateMachine.SetState(EnemyStateType.Attack);            
                return true;    
            }

            return false;
        }
    }
}
