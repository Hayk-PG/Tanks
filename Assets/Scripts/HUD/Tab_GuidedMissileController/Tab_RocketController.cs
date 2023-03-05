using UnityEngine;
using System;

public class Tab_RocketController : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup _canvasGroup;

    [SerializeField] [Space]
    private VariableJoystick _variableJoystick;

    public Vector3 Direction
    {
        get
        {
            return new Vector2(_variableJoystick.Horizontal, _variableJoystick.Vertical);
        }
    }

    public event Action<bool> onActivity;



    public void SetActivity(bool isActive)
    {
        GlobalFunctions.CanvasGroupActivity(_canvasGroup, isActive);

        onActivity?.Invoke(isActive);
    }
}
