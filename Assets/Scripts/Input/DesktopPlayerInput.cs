using UnityEngine;

namespace Input
{
    public class DesktopPlayerInput : IPlayerInput
    {
        public Vector2 Axes => new Vector2(UnityEngine.Input.GetAxis("Horizontal"), UnityEngine.Input.GetAxis("Vertical"));
        public Vector3 AimPosition => UnityEngine.Input.mousePosition;
        public bool IsFirePressed => UnityEngine.Input.GetButton("Fire1");
    }
}
