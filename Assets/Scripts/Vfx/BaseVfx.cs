using UnityEngine;

public abstract class BaseVfx : MonoBehaviour
{
    [SerializeField] private VfxType _vfxType;

    protected Transform _cachedTransform;
    private VfxLifeTimeChecker _vfxLifeTimeChecker;

    public VfxType Type => _vfxType;

    protected virtual void Awake()
    {
        _cachedTransform = transform;
        _vfxLifeTimeChecker = GetComponent<VfxLifeTimeChecker>();
        
        if (_vfxLifeTimeChecker != null)
        {
            _vfxLifeTimeChecker.OnLifeTimeExpired += Remove;
        }
    }

    private void OnDestroy()
    {
        if (_vfxLifeTimeChecker != null)
        {
            _vfxLifeTimeChecker.OnLifeTimeExpired -= Remove;
        }
    }
    
    public virtual void Init(Vector3 position, Quaternion rotation)
    {
        _cachedTransform.position = position;
        _cachedTransform.rotation = rotation;

        if (_vfxLifeTimeChecker != null)
        {
            _vfxLifeTimeChecker.Init();
        }
    }
    
    protected void Remove()
    {
        gameObject.SetActive(false);
    }
}
