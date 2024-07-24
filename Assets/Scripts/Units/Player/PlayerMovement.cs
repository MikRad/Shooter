using Input;
using Units.Animation;
using UnityEngine;

namespace Units.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Base settings")]
        [SerializeField] private Transform _bodyTransform;
        [SerializeField] private float _speed = 15f;
    
        private Animator _animator;
        private Rigidbody2D _rBody;
        private Transform _cachedTransform;
        private Camera _mainCamera;
        private IPlayerInput _playerInput;

        private void Start()
        {
            _rBody = GetComponent<Rigidbody2D>();
            _animator = GetComponentInChildren<Animator>();
            _cachedTransform = transform;
            _mainCamera = Camera.main;
        }

        private void Update()
        {
            Move();
            Rotate();
        }

        public void Init(IPlayerInput inputService)
        {
            _playerInput = inputService;
        }
    
        public void Stop()
        {
            _rBody.velocity = Vector2.zero;
            _rBody.angularVelocity = 0;
        
            _animator.SetFloat(UnitAnimationIdHelper.GetId(UnitAnimationState.Move), 0);
        }
    
        private void Move()
        {
            Vector2 direction = Vector2.ClampMagnitude(_playerInput.Axes, 1f);

            _rBody.velocity = direction * _speed;

            _animator.SetFloat(UnitAnimationIdHelper.GetId(UnitAnimationState.Move), direction.magnitude);
        }

        private void Rotate()
        {
            Vector3 aimPosition = _mainCamera.ScreenToWorldPoint(_playerInput.AimPosition);
            aimPosition.z = 0;
            Vector3 lookDirection = aimPosition - _cachedTransform.position;

            _bodyTransform.up = (Vector2) lookDirection;
        }
    }
}