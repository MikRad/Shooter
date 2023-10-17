using System;
using System.Collections.Generic;

public class DiContainer
{
    private readonly DiContainer _parent;
    private readonly Dictionary<Type, object> _singletons = new Dictionary<Type, object>();
    private readonly Dictionary<Type, DiGroup> _groups = new Dictionary<Type, DiGroup>();

    public DiContainer(DiContainer parent = null)
    {
        _parent = parent;
    }

    public void Register<T>(T instance)
    {
        _singletons.Add(typeof(T), instance);
    }
    
    public void Register<T>(T instance, string key)
    {
        Type type = typeof(T);
        if (!_groups.ContainsKey(type))
            _groups.Add(type, new DiGroup());

        _groups[type].Register(key, instance);
    }

    public T Resolve<T>()
    {
        if (_singletons.TryGetValue(typeof(T), out object instance))
            return (T)instance;
        if (_parent != null)
            return _parent.Resolve<T>();
        
        throw new InvalidOperationException($"Dependency {typeof(T)} not found");
    }

    public T Resolve<T>(string key)
    {
        if (!_groups.TryGetValue(typeof(T), out DiGroup group))
            throw new InvalidOperationException($"Dependency group {typeof(T)} not found");
        if (group.TryResolve(key, out object instance))
            return (T)instance;
        if (_parent != null)
            return _parent.Resolve<T>();
        
        throw new InvalidOperationException($"Dependency {typeof(T)} not found");
    }
}
