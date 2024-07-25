using UnityEngine;
using Vfx;

namespace Events.Fx
{
    public struct VfxNeededEvent : IEvent
    {
        public VfxType VfxType { get; private set; }
        public Transform TargetTransform { get; private set; }
    
        public VfxNeededEvent(VfxType vfxType, Transform targetTransform)
        {
            VfxType = vfxType;
            TargetTransform = targetTransform;
        }
    }
}
