using Photon.Pun;
using System;
using UnityEngine;

public class PlayerJoystick : MonoBehaviourPun
{
    private FixedJoystick _horizontalJoystick;
    private FixedJoystick _verticalJoystick;

    public Action<float> OnHorizontalJoystick { get; set; }
    public Action<float> OnVerticalJoystick { get; set; }


    protected virtual void Awake()
    {
        _horizontalJoystick = GameObject.Find(Names.HorizontalJoystick).GetComponent<FixedJoystick>();
        _verticalJoystick = GameObject.Find(Names.VerticalJoystick).GetComponent<FixedJoystick>();
    }

    private void Update()
    {
        OnHorizontalJoystick?.Invoke(_horizontalJoystick.Horizontal);
        OnVerticalJoystick?.Invoke(_verticalJoystick.Vertical);
    }
}
