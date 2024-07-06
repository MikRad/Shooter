using UnityEngine;

public class UnitFxHolder : MonoBehaviour
{
    [Header("Sfx")]
    [SerializeField] private SfxType[] _damagedSfxTypes;
    [SerializeField] private SfxType[] _deathSfxTypes;
    [SerializeField] private SfxType[] _shootSfxTypes;
    
    [Header("Vfx")]
    [SerializeField] private VfxType[] _damagedVfxTypes;

    protected AudioController _audioController;
    protected VfxSpawner _vfxSpawner;

    public void Init(AudioController audioController, VfxSpawner vfxSpawner)
    {
        _audioController = audioController;
        _vfxSpawner = vfxSpawner;
    }

    public void AddDamagedVfx(Vector3 position, Quaternion rotation)
    {
        PlayRandomVfx(_damagedVfxTypes, position, rotation);
    }
    
    public void PlayDamagedSfx()
    {
        PlayRandomSfx(_damagedSfxTypes);
    }
    
    public void PlayDeathSfx()
    {
        PlayRandomSfx(_deathSfxTypes);
    }
    
    public void PlayShootSfx()
    {
        PlayRandomSfx(_shootSfxTypes);
    }

    public void PlayHealthCollectSfx()
    {
        _audioController.PlaySfx(SfxType.HealthCollect);    
    }
    
    public void PlayGunMagazineCollectSfx()
    {
        _audioController.PlaySfx(SfxType.GunMagazineCollect);    
    }
    
    public void PlayNoAmmoSfx()
    {
        _audioController.PlaySfx(SfxType.NoAmmo);    
    }

    private void PlayRandomSfx(SfxType[] sfxTypeArray)
    {
        if (IsNotEmpty(sfxTypeArray))
        {
            int rndIdx = Random.Range(0, sfxTypeArray.Length);
            _audioController.PlaySfx(sfxTypeArray[rndIdx]);
        }
    }
    
    private void PlayRandomVfx(VfxType[] vfxTypeArray, Vector3 position, Quaternion rotation)
    {
        if (IsNotEmpty(vfxTypeArray))
        {
            int rndIdx = Random.Range(0, vfxTypeArray.Length);
            _vfxSpawner.SpawnVfx(vfxTypeArray[rndIdx], position, rotation);
        }
    }
    
    private bool IsNotEmpty<T>(T[] array)
    {
        return (array != null && array.Length > 0);
    }
}
