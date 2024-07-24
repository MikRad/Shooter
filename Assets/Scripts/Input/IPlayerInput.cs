using UnityEngine;

namespace Input
{
    public interface IPlayerInput
    {
        Vector2 Axes { get; }
        Vector3 AimPosition { get; }
        bool IsFirePressed { get; }
    }
}
