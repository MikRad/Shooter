using System;

public interface IBossConditionChecker
{
    event Action OnDied;
    event Action<float> OnHealthChanged;
    event Action<bool> OnActivated;
}
