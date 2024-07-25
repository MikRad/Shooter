using Audio;

namespace Events
{
    public struct SfxNeededEvent : IEvent
    {
        public SfxType SfxType { get; private set; }
    
        public SfxNeededEvent(SfxType sfxType)
        {
            SfxType = sfxType;
        }
    }
}
