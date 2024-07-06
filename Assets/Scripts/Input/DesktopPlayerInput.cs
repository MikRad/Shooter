using UnityEngine;

public class DesktopPlayerInput : IPlayerInput
{
    public Vector2 Axes => new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    public Vector3 AimPosition => Input.mousePosition;
    public bool IsFirePressed => Input.GetButton("Fire1");
}
