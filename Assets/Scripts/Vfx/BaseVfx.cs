using UnityEngine;

public abstract class BaseVfx : MonoBehaviour
{
    [SerializeField] private VfxType _vfxType;
    
    private Transform _cachedTransform;

    public VfxType Type => _vfxType;

    private void Awake()
    {
        _cachedTransform = transform;
    }

    public void Init(Vector3 position, Quaternion rotation)
    {
        _cachedTransform.position = position;
        _cachedTransform.rotation = rotation;
    }
    
    protected void Remove()
    {
        gameObject.SetActive(false);
    }
}
