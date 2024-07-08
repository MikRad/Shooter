public class EventHandler
{
    public object Callback { get; }
    public int Priority { get; }
    
    public EventHandler(object callback, int priority)
    {
        Callback = callback;
        Priority = priority;
    }
}
