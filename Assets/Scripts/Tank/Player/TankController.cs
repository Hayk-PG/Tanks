using System;
using UnityEngine;

public class TankController : MonoBehaviour
{
    private delegate void Subscription(bool isTrue);
    private Subscription _subscribeToPlayerJoystick;
    private Subscription _unsubscribeFromPlayerJosytick;
    private Subscription _subscribeToPlayerShootButton;
    private Subscription _unsubscribeFromPlayerShootButton;

    [SerializeField] private BasePlayer _basePlayer;

    internal BasePlayer BasePlayer
    {
        get => _basePlayer;
        private set => _basePlayer = value;
    }
    private PlayerJoystick _playerJoystick;
    private PlayerShootButton _playerShootButton;

    internal Action<float> OnHorizontalJoystick { get; set; }
    internal Action<float> OnVerticalJoystick { get; set; }
    internal Action<bool> OnShootButtonPointer { get; set; }
    internal Action<bool> OnShootButtonClick { get; set; }
    internal Action OnInitialize { get; set; }


    private void Awake()
    {
        _unsubscribeFromPlayerJosytick = delegate (bool isTrue)
        {
            if (isTrue)
            {
                _playerJoystick.OnHorizontalJoystick -= OnHorizontalJoystick;
                _playerJoystick.OnVerticalJoystick -= OnVerticalJoystick;
            };
        };
        _subscribeToPlayerJoystick = delegate (bool isTrue)
        {
            if (isTrue)
            {
                _playerJoystick.OnHorizontalJoystick += OnHorizontalJoystick;
                _playerJoystick.OnVerticalJoystick += OnVerticalJoystick;
            }
        };
        _subscribeToPlayerShootButton = delegate (bool isTrue)
        {
            if (isTrue)
            {
                _playerShootButton.OnShootButtonPointer += OnShootButtonPointer;
                _playerShootButton.OnShootButtonClick += OnShootButtonClick;
            }
        };
        _unsubscribeFromPlayerShootButton = delegate (bool isTrue)
        {
            if (isTrue)
            {
                _playerShootButton.OnShootButtonPointer -= OnShootButtonPointer;
                _playerShootButton.OnShootButtonClick -= OnShootButtonClick;
            }
        };
    }

    private void OnDisable()
    {
        _unsubscribeFromPlayerJosytick?.Invoke(_playerJoystick != null);
        _unsubscribeFromPlayerShootButton?.Invoke(_playerShootButton != null);
    }

    public void GetTankControl(BasePlayer player)
    {
        BasePlayer = player;

        if(BasePlayer != null)
        {
            _playerJoystick = BasePlayer.GetComponent<PlayerJoystick>();
            _playerShootButton = BasePlayer.GetComponent<PlayerShootButton>();
            _subscribeToPlayerJoystick?.Invoke(_playerJoystick != null);
            _subscribeToPlayerShootButton?.Invoke(_playerShootButton != null);

            OnInitialize?.Invoke();
        }
    }
}
