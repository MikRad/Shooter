using System.Collections.Generic;

namespace Units.Enemy.EnemyStates
{
    public class EnemyStateMachine
    {
        private EnemyState _currentState;
        private readonly Dictionary<EnemyStateType, EnemyState> _statesMap = new Dictionary<EnemyStateType, EnemyState>();

        public void Update()
        {
            _currentState.Update();    
        }

        public void SetState(EnemyStateType stateType)
        {
            if (_statesMap.TryGetValue(stateType, out EnemyState state))
            {
                _currentState?.OnExit();
                _currentState = state;
                _currentState.OnEnter();
            }
        }

        public void AddState(EnemyStateType stateType, EnemyState state)
        {
            _statesMap[stateType] = state;
        }
    }
}
