using Audio;
using Events.Services;
using Events.Services.Fx;
using UnityEngine;

namespace Vfx.Services
{
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
            EventBus.Get.RaiseEvent(this, new SfxNeededEvent(SfxType.HealthCollect));
        }
    
        public void AddGunMagazineCollectSfx()
        {
            EventBus.Get.RaiseEvent(this, new SfxNeededEvent(SfxType.GunMagazineCollect));
        }
    
        public void AddNoAmmoSfx()
        {
            EventBus.Get.RaiseEvent(this, new SfxNeededEvent(SfxType.NoAmmo));
        }

        private void AddRandomSfx(SfxType[] sfxTypeArray)
        {
            if (IsNotEmpty(sfxTypeArray))
            {
                int rndIdx = Random.Range(0, sfxTypeArray.Length);
            
                EventBus.Get.RaiseEvent(this, new SfxNeededEvent(sfxTypeArray[rndIdx]));
            }
        }
    
        private void AddRandomVfx(VfxType[] vfxTypeArray, Transform target)
        {
            if (IsNotEmpty(vfxTypeArray))
            {
                int rndIdx = Random.Range(0, vfxTypeArray.Length);
            
                EventBus.Get.RaiseEvent(this, new VfxNeededEvent(vfxTypeArray[rndIdx], target));
            }
        }
    
        private bool IsNotEmpty<T>(T[] array)
        {
            return (array != null && array.Length > 0);
        }
    }
}
