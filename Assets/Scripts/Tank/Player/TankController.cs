using System;
using UnityEngine;

public class TankController : MonoBehaviour
{
    private BasePlayer _basePlayer;
    private Controllers _controllers; 
    private ShootButton _shootButton;

    internal BasePlayer BasePlayer
    {
        get => _basePlayer;
        private set => _basePlayer = value;
    }
    internal Action<Vector2> OnControllers { get; set; }
    internal Action<float> OnMovementDirection { get; set; } 
    internal Action<bool> OnShootButtonClick { get; set; }
    internal Action OnInitialize { get; set; }


    private void Awake()
    {
        _controllers = FindObjectOfType<Controllers>();
        _shootButton = FindObjectOfType<ShootButton>();
    }

    public void GetTankControl(BasePlayer player)
    {
        BasePlayer = player;

        if(BasePlayer != null)
        {
            _controllers.OnControllers += OnControllers;
            _controllers.OnHorizontalJoystick += OnMovementDirection;
            _shootButton.OnClick += OnShootButtonClick;
            OnInitialize?.Invoke();
        }
    }
}
