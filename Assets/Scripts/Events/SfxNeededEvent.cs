using Audio;

namespace Events.Services
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
