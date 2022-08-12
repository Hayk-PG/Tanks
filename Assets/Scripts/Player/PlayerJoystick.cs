using Photon.Pun;
using System;
using UnityEngine;

public class PlayerJoystick : MonoBehaviourPun
{
    private FixedJoystick _horizontalJoystick;
    private FixedJoystick _rightJoystick;

    public Action<float> OnHorizontalJoystick { get; set; }
    public Action<Vector2> OnRightJoystick { get; set; }


    protected virtual void Awake()
    {
        _horizontalJoystick = GameObject.Find(Names.HorizontalJoystick).GetComponent<FixedJoystick>();
        _rightJoystick = GameObject.Find(Names.VerticalJoystick).GetComponent<FixedJoystick>();
    }

    private void Update()
    {
        OnHorizontalJoystick?.Invoke(_horizontalJoystick.Horizontal);
        OnRightJoystick?.Invoke(new Vector2(_rightJoystick.Horizontal, _rightJoystick.Vertical));
    }
}
