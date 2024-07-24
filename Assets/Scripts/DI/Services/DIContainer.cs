using System;
using System.Collections.Generic;

namespace DI.Services
{
    public class DIContainer
    {
        private readonly DIContainer _parent;
        private readonly Dictionary<(string, Type), DIEntry> _dependenciesMap = new Dictionary<(string, Type), DIEntry>();
        private readonly HashSet<(string, Type)> _cachedResolvingKeys = new HashSet<(string, Type)>();

        public DIContainer(DIContainer parent = null)
        {
            _parent = parent;
        }

        public void RegisterSingleton<T>(Func<DIContainer, T> factory)
        {
            RegisterSingleton(null, factory);    
        }
    
        public void RegisterSingleton<T>(string tag, Func<DIContainer, T> factory)
        {
            var key = (tag, typeof(T));
        
            Register(key, factory, true);
        }
    
        public void RegisterTransient<T>(Func<DIContainer, T> factory)
        {
            RegisterTransient(null, factory);    
        }
    
        public void RegisterTransient<T>(string tag, Func<DIContainer, T> factory)
        {
            var key = (tag, typeof(T));
        
            Register(key, factory, false);
        }

        public void RegisterInstance<T>(T instance)
        {
            RegisterInstance(null, instance);
        }
    
        public void RegisterInstance<T>(string tag, T instance)
        {
            var key = (tag, typeof(T));
        
            if (_dependenciesMap.ContainsKey(key))
            {
                throw new Exception(
                    $"Dependency with tag {key.Item1} and type {key.Item2.FullName} has already registered !");
            }

            _dependenciesMap[key] = new DIEntrySingleton<T>(this, instance);
        }

        public T Resolve<T>(string tag = null)
        {
            var key = (tag, typeof(T));

            if (_cachedResolvingKeys.Contains(key))
            {
                throw new Exception($"Cyclic dependencies for tag {key.Item1} and type {key.Item2} !");
            }
        
            _cachedResolvingKeys.Add(key);

            try
            {
                if (_dependenciesMap.TryGetValue(key, out DIEntry dependency))
                {
                    return dependency.Resolve<T>();
                }

                if (_parent != null)
                {
                    return _parent.Resolve<T>(tag);
                }
            }
            finally
            {
                _cachedResolvingKeys.Remove(key);
            }
        
            throw new Exception(
                $"Couldn't find dependency with tag {key.Item1} and type {key.Item2.FullName} has already registered !");
        }
    
        private void Register<T>((string, Type) key, Func<DIContainer, T> factory, bool isSingleton)
        {
            if (_dependenciesMap.ContainsKey(key))
            {
                throw new Exception(
                    $"Dependency with tag {key.Item1} and type {key.Item2.FullName} has already registered !");
            }

            if (isSingleton)
            {
                _dependenciesMap[key] = new DIEntrySingleton<T>(this, factory);
            }
            else
            {
                _dependenciesMap[key] = new DIEntryTransient<T>(this, factory);
            }
        }
    }
}
