using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Base settings")]
    [SerializeField] private Transform _bodyTransform;
    [SerializeField] private float _speed = 15f;
    
    private Animator _animator;
    private Rigidbody2D _rBody;
    private Transform _cachedTransform;
    private Camera _mainCamera;

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

    public void Stop()
    {
        _rBody.velocity = Vector2.zero;
        _rBody.angularVelocity = 0;
        
        _animator.SetFloat(UnitAnimationIdHelper.GetId(UnitAnimationState.Move), 0);
    }
    
    private void Move()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector2 direction = Vector2.ClampMagnitude(new Vector2(horizontal, vertical), 1f);

        _rBody.velocity = direction * _speed;

        _animator.SetFloat(UnitAnimationIdHelper.GetId(UnitAnimationState.Move), direction.magnitude);
    }

    private void Rotate()
    {
        Vector3 mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        Vector3 direction = mousePosition - _cachedTransform.position;

        _bodyTransform.up = (Vector2) direction;
    }
}