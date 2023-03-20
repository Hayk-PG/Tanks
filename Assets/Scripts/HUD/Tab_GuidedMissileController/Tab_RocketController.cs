using UnityEngine;
using System;

public class Tab_RocketController : MonoBehaviour, IEndGame
{
    [SerializeField]
    private CanvasGroup _canvasGroup;

    [SerializeField] [Space]
    private FixedJoystick _fixedJoystick;

    public Vector3 Direction
    {
        get
        {
            return new Vector2(_fixedJoystick.Horizontal, _fixedJoystick.Vertical);
        }
    }

    public event Action<bool> onActivity;

    public void SetActivity(bool isActive)
    {
        GlobalFunctions.CanvasGroupActivity(_canvasGroup, isActive);

        onActivity?.Invoke(isActive);
    }

    public void OnGameEnd(object[] data = null)
    {
        SetActivity(false);
    }
}
