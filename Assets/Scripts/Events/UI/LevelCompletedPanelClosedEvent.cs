using UI;

namespace Events.UI
{
    public struct LevelCompletedPanelClosedEvent : IEvent
    {
        public LevelCompletedPanel.UserAction Action { get; private set; }
    
        public LevelCompletedPanelClosedEvent(LevelCompletedPanel.UserAction action)
        {
            Action = action;
        }
    }
}
