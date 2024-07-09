using UnityEngine;

[RequireComponent(typeof(PickupItemAnimator))]
public abstract class BasePickupItem : MonoBehaviour
{
    [Header("Base settings")]
    [SerializeField] private PickupItemType _pickupItemType;

    private Transform _cachedTransform;
    
    public PickupItemType Type => _pickupItemType;

    private void Awake()
    {
        _cachedTransform = transform;
    }

    public void Init(Vector3 position, Quaternion rotation)
    {
        _cachedTransform.position = position;
        _cachedTransform.rotation = rotation;
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Player player))
            HandleCollecting(player);
    }

    protected void Remove()
    {
        gameObject.SetActive(false);
    }
    
    protected abstract void HandleCollecting(Player player);
}
