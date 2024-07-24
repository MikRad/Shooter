using System;
using DI.Services;

namespace DI
{
    public sealed class DIEntryTransient<T> : DIEntry<T>
    {
        public DIEntryTransient(DIContainer diContainer, Func<DIContainer, T> factory) : base(diContainer, factory)
        {
        }

        public override T Resolve()
        {
            return Factory(DiContainer);
        }
    }
}
