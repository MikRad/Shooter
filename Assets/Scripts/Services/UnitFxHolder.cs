using UnityEngine;

public class UnitFxHolder : MonoBehaviour
{
    [Header("Sfx")]
    [SerializeField] private SfxType[] _damagedSfxTypes;
    [SerializeField] private SfxType[] _deathSfxTypes;
    [SerializeField] private SfxType[] _shootSfxTypes;
    
    [Header("Vfx")]
    [SerializeField] private VfxType[] _damagedVfxTypes;

    public void AddDamagedVfx(Transform target)
    {
        AddRandomVfx(_damagedVfxTypes, target);
    }
    
    public void AddDamagedSfx()
    {
        AddRandomSfx(_damagedSfxTypes);
    }
    
    public void AddDeathSfx()
    {
        AddRandomSfx(_deathSfxTypes);
    }
    
    public void AddShootSfx()
    {
        AddRandomSfx(_shootSfxTypes);
    }

    public void AddHealthCollectSfx()
    {
        SfxNeededEvent ev = new SfxNeededEvent(SfxType.HealthCollect);
        EventBus.Get.RaiseEvent(this, ref ev);
    }
    
    public void AddGunMagazineCollectSfx()
    {
        SfxNeededEvent ev = new SfxNeededEvent(SfxType.GunMagazineCollect);
        EventBus.Get.RaiseEvent(this, ref ev);
    }
    
    public void AddNoAmmoSfx()
    {
        SfxNeededEvent ev = new SfxNeededEvent(SfxType.NoAmmo);
        EventBus.Get.RaiseEvent(this, ref ev);
    }

    private void AddRandomSfx(SfxType[] sfxTypeArray)
    {
        if (IsNotEmpty(sfxTypeArray))
        {
            int rndIdx = Random.Range(0, sfxTypeArray.Length);
            
            SfxNeededEvent ev = new SfxNeededEvent(sfxTypeArray[rndIdx]);
            EventBus.Get.RaiseEvent(this, ref ev);
        }
    }
    
    private void AddRandomVfx(VfxType[] vfxTypeArray, Transform target)
    {
        if (IsNotEmpty(vfxTypeArray))
        {
            int rndIdx = Random.Range(0, vfxTypeArray.Length);
            
            VfxNeededEvent ev = new VfxNeededEvent(vfxTypeArray[rndIdx], target);
            EventBus.Get.RaiseEvent(this, ref ev);
        }
    }
    
    private bool IsNotEmpty<T>(T[] array)
    {
        return (array != null && array.Length > 0);
    }
}
