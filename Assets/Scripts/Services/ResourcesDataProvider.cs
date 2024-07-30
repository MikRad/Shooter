using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

public class ResourcesDataProvider : IResourcesDataProvider
{
    private readonly Dictionary<Type, Object> _resourcesCache = new Dictionary<Type, Object>();
    private readonly Dictionary<Type, Object[]> _resourcesGroupCache = new Dictionary<Type, Object[]>();

    public T LoadResource<T>(string path) where T : Object
    {
        Type resourceType = typeof(T);
        
        if (_resourcesCache.TryGetValue(resourceType, out Object cachedResource))
        {
            return cachedResource as T;
        }

        T resource = Resources.Load<T>(path);
        if (resource == null)
        {
            Debug.LogError($"Failed to load resource of type {typeof(T)} at path: {path}");
        }
        else
        {
            _resourcesCache[resourceType] = resource;
        }

        return resource;
    }

    public T[] LoadAllResources<T>(string path) where T : Object
    {
        Type resourceType = typeof(T);
            
        if (_resourcesGroupCache.TryGetValue(resourceType, out Object[] cachedResources))
        {
            return cachedResources as T[];
        }

        T[] resources = Resources.LoadAll<T>(path);
        if (resources == null || resources.Length == 0)
        {
            Debug.LogError($"Failed to load resources of type {typeof(T)} at path: {path}");
        }
        else
        {
            _resourcesGroupCache[resourceType] = resources;
        }

        return resources;
    }

    public async Task<T> LoadResourceAsync<T>(string path) where T : Object
    {
        Type resourceType = typeof(T);
        
        if (_resourcesCache.TryGetValue(resourceType, out Object cachedResource))
        {
            return cachedResource as T;
        }

        ResourceRequest request = Resources.LoadAsync<T>(path);
        while (!request.isDone)
        {
            await Task.Yield();
        }

        T resource = request.asset as T;
        if (resource == null)
        {
            Debug.LogError($"Failed to load resource of type {typeof(T)} at path: {path}");
        }
        else
        {
            _resourcesCache[resourceType] = resource;
        }

        return resource;
    }
}
