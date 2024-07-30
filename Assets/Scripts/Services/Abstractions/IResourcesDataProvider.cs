using UnityEngine;

public interface IResourcesDataProvider
{
    T LoadResource<T>(string path) where T : Object;
    T[] LoadAllResources<T>(string path) where T : Object;
    System.Threading.Tasks.Task<T> LoadResourceAsync<T>(string path) where T : Object;    
}