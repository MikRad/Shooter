using UnityEngine;

namespace Vfx
{
    [RequireComponent(typeof(ParticleSystem))]
    public class BloodVfx : BaseVfx
    {
        private ParticleSystem _particles;
    
        protected override void Awake()
        {
            base.Awake();
        
            _particles = GetComponent<ParticleSystem>();
        }

        public override void Init(Vector3 position, Quaternion rotation)
        {
            base.Init(position, rotation);

            _particles.Play();
        }
    }
}
