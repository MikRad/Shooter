using Pathfinding;
using Units.Animation;
using UnityEngine;
using Utils;

namespace Units.Enemy
{
    [RequireComponent(typeof(AIDestinationSetter)), RequireComponent(typeof(AIPath)), RequireComponent(typeof(Seeker))]
    public class EnemyMovement : MonoBehaviour
    {
        [Header("Base settings")]
        [SerializeField] private Transform _bodyTransform;
        [SerializeField] private float _baseSpeed = 5f;
        [SerializeField] private float _patrolSpeedFactor = 0.5f;
        [SerializeField] private float _returnSpeedFactor = 0.8f;
        [SerializeField] private float _rageSpeedFactor = 1.2f;
        [SerializeField] private float _defaultPathUpdateInterval = 0.5f;
        [SerializeField] private float _patrolPathUpdateInterval = 1.0f;
        [SerializeField] private float _returnPathUpdateInterval = 1.0f;

        private Animator _animator;
        private Transform _cachedTransform;
        private Transform _targetTransform;
        private Rigidbody2D _rBody;

        private AIPath _aiPath;
        private AIDestinationSetter _aiDestinationSetter;

        private void Awake()
        {
            _cachedTransform = transform;
            _rBody = GetComponent<Rigidbody2D>();
            _aiPath = GetComponent<AIPath>();
            _animator = GetComponentInChildren<Animator>();
            _aiDestinationSetter = GetComponent<AIDestinationSetter>();
        
            CreateTargetTransform();
            _aiDestinationSetter.target = _targetTransform;

            _aiPath.maxAcceleration = 5f;
        }

        private void Update()
        {
            _animator.SetFloat(UnitAnimationIdHelper.GetId(UnitAnimationState.Move), _aiPath.velocity.magnitude);
            Rotate();
        }

        public void SetTargetPosition(Vector3 pos)
        {
            _targetTransform.position = pos;
        }

        public void StopMove()
        {
            _aiPath.maxSpeed = 0;
            _aiPath.enabled = false;
            _rBody.velocity = Vector2.zero;
            _rBody.isKinematic = true;

            _animator.SetFloat(UnitAnimationIdHelper.GetId(UnitAnimationState.Move), 0);
        }

        public void StartMove(EnemyMoveType moveType = EnemyMoveType.Usual)
        {
            _rBody.isKinematic = false;
            _aiPath.enabled = true;

            float maxSpeed;
            switch (moveType)
            {
                case EnemyMoveType.Patrol:
                    maxSpeed = _baseSpeed * _patrolSpeedFactor;
                    _aiPath.repathRate = _patrolPathUpdateInterval;
                    break;
                case EnemyMoveType.Return:
                    maxSpeed = _baseSpeed * _returnSpeedFactor;
                    _aiPath.repathRate = _returnPathUpdateInterval;
                    break;
                case EnemyMoveType.Rage:
                    maxSpeed = _baseSpeed * _rageSpeedFactor;
                    break;
                default:
                    maxSpeed = _baseSpeed;
                    _aiPath.repathRate = _defaultPathUpdateInterval;
                    break;
            }
        
            _aiPath.maxSpeed = maxSpeed;
        }
    
        private void Rotate()
        {
            Vector3 directionToPlayer = (_targetTransform.position - _cachedTransform.position).normalized;
            float angle = Vector3.Angle(directionToPlayer, _aiPath.velocity);
            
            if(angle > 45f)
            {
                _bodyTransform.up = _aiPath.velocity;
            }
            else
            {
                _bodyTransform.up = (Vector2)directionToPlayer;
            }
        }

        private void CreateTargetTransform()
        {
            GameObject target = new GameObject($"{name} target");
            _targetTransform = target.transform;
            _targetTransform.parent = TempPoints.Container;
        }
    }
}