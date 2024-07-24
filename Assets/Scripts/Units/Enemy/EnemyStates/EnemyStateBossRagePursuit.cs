using System;
using Units.Abstractions;
using UnityEngine;

namespace Units.Enemy.EnemyStates
{
    public class EnemyStateBossRagePursuit : EnemyStateRagePursuit
    {
        private readonly Action<bool> _activationCallback;
    
        public EnemyStateBossRagePursuit(EnemyUnit unit, EnemyStateMachine stateMachine, EnemyMovement unitMovement,
            Transform playerTransform, float rageDuration, Action<bool> activationCheckCallback)
            : base(unit, stateMachine, unitMovement, playerTransform, rageDuration)
        {
            _activationCallback = activationCheckCallback;
        }
    
        public override void OnEnter()
        {
            base.OnEnter();
        
            _activationCallback(true);
        }
    }
}