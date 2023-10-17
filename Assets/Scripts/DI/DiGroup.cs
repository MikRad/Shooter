using System.Collections.Generic;

public class DiGroup
{
    private readonly Dictionary<string, object> _instances = new Dictionary<string, object>();
    
    public void Register<T>(string key, T instance)
    {
        _instances.Add(key, instance);
    }

    public bool TryResolve(string key, out object instance)
    {
        return _instances.TryGetValue(key, out instance);
    }
}
