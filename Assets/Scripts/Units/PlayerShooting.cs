using UnityEngine;

public class PlayerShooting : BaseShooting
{
    [Header("Ammo settings")]
    [SerializeField] private int _ammoMax = 100;
    [Header("Aim settings")]
    [SerializeField] private GameObject _aimPrefab;
    [Range(5f, 15f)]
    [SerializeField] private float _aimSensitivity = 10f;
    [Range(0.1f, 1f)]
    [SerializeField] private float _aimDamping = 0.5f;
    
    private int _currentAmmo;

    private Transform _aim;
    private Vector3 _aimMovePos;
    private Vector3 _aimMoveVel;
    
    private Camera _mainCamera;
    
    private SpringUtils.tDampedSpringMotionParams _springParams = new SpringUtils.tDampedSpringMotionParams();
    
    public bool HasAmmo => (_currentAmmo > 0);
    public bool HasMaxAmmo => _currentAmmo == _ammoMax;
    public float AmmoFullness => (float)_currentAmmo / _ammoMax;

    private void Start()
    {
        _currentAmmo = _ammoMax;
        _mainCamera = Camera.main;
        
        _aim = Instantiate(_aimPrefab, TempPoints.Container).transform;
    }

    private void Update()
    {
        UpdateAimPosition();
    }

    public override void Shoot()
    {
        ShootAt(_aim.position);
        
        _currentAmmo--;
    }

    public void AddAmmo(int ammoAmount)
    {
        _currentAmmo += ammoAmount;
        ClampAmmoValue();
    }
    
    private void UpdateAimPosition()
    {
        Vector2 mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        
        SpringUtils.CalcDampedSpringMotionParams(ref _springParams, Time.deltaTime, _aimSensitivity, _aimDamping);
        SpringUtils.UpdateDampedSpringMotion(ref _aimMovePos, ref _aimMoveVel, mousePosition, _springParams);
        
        _aim.position = _aimMovePos;
    }
    
    private void ClampAmmoValue()
    {
        _currentAmmo = Mathf.Clamp(_currentAmmo, 0, _ammoMax);
    }
}
