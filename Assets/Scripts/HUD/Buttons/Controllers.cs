using System;
using UnityEngine;

public class Controllers : MonoBehaviour
{
    private enum Buttons {None, Up, Down, Left, Right}
    private Buttons _buttons;

    private FixedJoystick _movementJoystick;

    public Action<Vector2> OnControllers { get; set; }
    public Action<float> OnHorizontalJoystick { get; set; }


    private void Awake()
    {
        _movementJoystick = Get<FixedJoystick>.From(GameObject.Find("HorizontalJoystick"));
    }

    private void Update()
    {
        switch (_buttons)
        {
            case Buttons.None: OnControllers?.Invoke(new Vector2(0, 0)); break;
            case Buttons.Up: OnControllers?.Invoke(new Vector2(0, 1)); break;
            case Buttons.Down: OnControllers?.Invoke(new Vector2(0, -1)); break;
            case Buttons.Left: OnControllers?.Invoke(new Vector2(-1, 0)); break;
            case Buttons.Right: OnControllers?.Invoke(new Vector2(1, 0)); break;
        }

        OnHorizontalJoystick?.Invoke(_movementJoystick.Horizontal);
    }

    public void OnPointerDown(int index)
    {
        _buttons = (Buttons)index;
    }

    public void OnPointerUp()
    {
        _buttons = Buttons.None;
    }
}
