using System;
using DI.Services;

namespace DI
{
    public class DIEntrySingleton<T> : DIEntry<T>
    {
        private T _instance;
    
        public DIEntrySingleton(DIContainer diContainer, Func<DIContainer, T> factory) : base(diContainer, factory)
        {
        }

        public DIEntrySingleton(DIContainer diContainer, T instance) : base(diContainer, null)
        {
            _instance = instance;
        }
    
        public override T Resolve()
        {
            if (_instance == null && Factory != null)
            {
                _instance = Factory(DiContainer);
            }

            return _instance;
        }
    }
}
