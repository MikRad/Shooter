using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    [Header("Base settings")]
    [SerializeField] private BulletType _bulletType;
    [SerializeField] private float _speed = 75f;
    [SerializeField] private float _lifeTime = 2f;

    private Transform _cachedTransform;
    private Rigidbody2D _rBody;
    private float _currentLifeTime;

    public BulletType Type => _bulletType;
    
    private void Awake()
    {
        _cachedTransform = transform;
        _rBody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        UpdateLifeTime();
    }

    public void Init(Vector3 position, Quaternion rotation)
    {
        _currentLifeTime = _lifeTime;
        _cachedTransform.position = position;
        _cachedTransform.rotation = rotation;
        
        _rBody.velocity = _cachedTransform.up * _speed;
    }
    
    private void UpdateLifeTime()
    {
        _currentLifeTime -= Time.deltaTime;
        if (_currentLifeTime < 0)
        {
            Remove();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Remove();
    }

    private void Remove()
    {
        gameObject.SetActive(false);
    }
}