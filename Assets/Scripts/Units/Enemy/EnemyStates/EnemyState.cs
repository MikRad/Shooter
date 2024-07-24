using Units.Abstractions;
using UnityEngine;

namespace Units.Enemy.EnemyStates
{
    public abstract class EnemyState
    {
        protected readonly EnemyUnit _unit;
        protected readonly EnemyMovement _unitMovement;
        protected readonly Transform _unitTransform;
        protected readonly EnemyStateMachine _stateMachine;

        protected EnemyState(EnemyUnit unit, EnemyStateMachine stateMachine, EnemyMovement unitMovement)
        {
            _unit = unit;
            _unitTransform = _unit.transform;
            _stateMachine = stateMachine;
            _unitMovement = unitMovement;
        }

        public abstract void Update();
    
        public virtual void OnEnter()
        {
        }
    
        public virtual void OnExit()
        {
        }
    
        protected abstract bool CheckTransitionConditions();
    }
}
