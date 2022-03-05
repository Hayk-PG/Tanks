using System;
using UnityEngine;

public class UITouchMovement : MonoBehaviour
{
    public event Action OnCameraCanMove;

    [SerializeField]
    private FixedJoystick _horJoystick, _vertJoystick;

    private delegate bool Checker();
    private Checker _isJoysticksMoving;


    private void Start()
    {
        _isJoysticksMoving = delegate { return _horJoystick.Horizontal != 0 || _vertJoystick.Vertical != 0; };
    }

    private void Update()
    {
        Conditions<bool>.Compare(_isJoysticksMoving(), null, () => OnCameraCanMove?.Invoke());
    }
}
