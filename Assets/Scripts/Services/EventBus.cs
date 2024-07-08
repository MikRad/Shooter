using System;
using System.Collections.Generic;
using System.Linq;

public delegate void EventCallback<T>(ref T raisedEvent) where T : IEvent;

public class EventBus
{
    private readonly Dictionary<Type, List<EventHandler>> _handlersMap = new Dictionary<Type, List<EventHandler>>();
    private static EventBus _instance;

    public static EventBus Get => _instance ??= new EventBus();

    public void Subscribe<TEvent>(EventCallback<TEvent> callback, int priority = 0) where TEvent : IEvent
    {
        Type eventType = typeof(TEvent);
        
        if (!_handlersMap.TryGetValue(eventType, out List<EventHandler> handlers))
        {
            handlers = new List<EventHandler>();
            _handlersMap[eventType] = handlers;
        }
        
        handlers.Add(new EventHandler(callback, priority));
        handlers = handlers.OrderByDescending(h => h.Priority).ToList();
        
        _handlersMap[eventType] = handlers;
    }
    
    public void Unsubscribe<TEvent>(EventCallback<TEvent> callback) where TEvent : IEvent
    {
        Type eventType = typeof(TEvent); 
        
        if (_handlersMap.TryGetValue(eventType, out List<EventHandler> handlers))
        {
            EventHandler handlerToRemove = handlers.FirstOrDefault(h => h.Callback.Equals(callback));
            
            if (handlerToRemove != null)
            {
                handlers.Remove(handlerToRemove);
            }
        }
    }

    public void RaiseEvent<TEvent>(object sender, ref TEvent ev) where TEvent : IEvent
    {
        Type eventType = ev.GetType();
        
        if (_handlersMap.TryGetValue(eventType, out List<EventHandler> handlers))
        {
            foreach (EventHandler handler in handlers)
            {
                EventCallback<TEvent> callback = handler.Callback as EventCallback<TEvent>;
                callback?.Invoke(ref ev);
            }
        }
    }

    private EventBus() { }
}