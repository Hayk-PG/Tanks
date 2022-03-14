﻿using UnityEngine;
using System;

public class PlayerRequestAirSupport : MonoBehaviour
{
    private bool _isAirSupportRequested;

    private Bomber _bomber;

    private AirSupport _airSupport;
    private SupportsTabCustomization _supportsTabCustomization;
    private PlayerTurn _playerTurn;
    private ShootButton _shootButton;

    private delegate Vector3 Vector();
    private delegate Quaternion Rotation();
    private Vector _bomberPosition;
    private Rotation _bomberRotation;

    public IScore _iScore;

    private Vector3 _rightSpwnPos = new Vector3(15, 3, 0);
    private Vector3 _leftSpwnPos = new Vector3(-15, 3, 0);


    private void Awake()
    {
        _playerTurn = Get<PlayerTurn>.From(gameObject);
        _iScore = Get<IScore>.From(gameObject);

        _airSupport = FindObjectOfType<AirSupport>();
        _supportsTabCustomization = FindObjectOfType<SupportsTabCustomization>();
        _shootButton = FindObjectOfType<ShootButton>();
    }

    private void Start()
    {
        _bomberPosition = delegate
        {
            return _playerTurn.MyTurn == TurnState.Player1 ? _rightSpwnPos : _leftSpwnPos;
        };

        _bomberRotation = delegate
        {
            return _playerTurn.MyTurn == TurnState.Player1 ? Quaternion.Euler(-90, -90, 0) : Quaternion.Euler(-90, 90, 0);
        };
    }

    private void OnEnable()
    {
        _airSupport.OnRequestAirSupport += OnRequestAirSupport;
        _supportsTabCustomization.OnCallBomber += CallBomber;
        _shootButton.OnClick += OnShootButtonClick;
    }
   
    private void OnDisable()
    {
        _airSupport.OnRequestAirSupport += OnRequestAirSupport;
        _supportsTabCustomization.OnCallBomber -= CallBomber;
        _shootButton.OnClick -= OnShootButtonClick;
    }

    private void CallBomber()
    {
        if (_playerTurn.IsMyTurn)
        {
            _airSupport.CallOnRequestAirSupport();
            _isAirSupportRequested = true;
        }
    }

    private void OnRequestAirSupport(Bomber bomber, Action<AirSupport.InstantiateProperties> ActivateBomber)
    {       
        ActivateBomber?.Invoke(new AirSupport.InstantiateProperties(_bomberPosition(), _bomberRotation()));

        _bomber = bomber;
        _bomber.distanceX = _rightSpwnPos.x;
    }

    private void OnShootButtonClick(bool isTrue)
    {
        if (isTrue && _isAirSupportRequested)
        {
            _isAirSupportRequested = false;
            _bomber?.DropBomb(_iScore);
        }
    }
}