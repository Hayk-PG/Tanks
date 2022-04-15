using System;
using UnityEngine;

public class TankController : MonoBehaviour
{
    private delegate void Subscription(bool isTrue);
    private Subscription _subscribeToPlayerJoystick;
    private Subscription _unsubscribeFromPlayerJosytick;
    
    private PhotonPlayerController _player;
    private PlayerJoystick _playerJoystick;

    internal Action<float> OnHorizontalJoystick { get; set; }
    internal Action<float> OnVerticalJoystick { get; set; }


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
            _playerJoystick.OnHorizontalJoystick += OnHorizontalJoystick;
            _playerJoystick.OnVerticalJoystick += OnVerticalJoystick;
        };
    }

    private void OnDisable()
    {
        _unsubscribeFromPlayerJosytick?.Invoke(_playerJoystick != null);
    }

    public void GetTankControl(PhotonPlayerController player)
    {
        _player = player;

        if(_player != null)
        {
            _playerJoystick = _player.GetComponent<PlayerJoystick>();
            _subscribeToPlayerJoystick?.Invoke(_playerJoystick != null);
        }
    }
}
