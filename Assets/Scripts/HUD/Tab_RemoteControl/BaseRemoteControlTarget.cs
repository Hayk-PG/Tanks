﻿using System;
using UnityEngine;

public class BaseRemoteControlTarget : MonoBehaviour
{
    [SerializeField] protected RectTransform _canvasRectransform;
    [SerializeField] protected CanvasGroup _mainTabCanvasGroup;

    protected RectTransform _rectTransform;
    protected Animator _animator;
    protected CanvasGroup _canvasGroup;
    protected Ray _ray;
    protected RaycastHit _raycastHit;
    protected Camera _mainCamera;
    protected TurnController _turnController;
    protected GameplayAnnouncer _gameplayAnnouncer;
    protected bool _hasGameStartBeenAnnounced;
    protected bool _isPlayingAnimation;

    public Action<bool> OnRemoteControlTargetActivity { get; set; }
    public Action<Vector3> OnSet { get; set; }



    protected virtual void Awake()
    {
        _rectTransform = Get<RectTransform>.From(gameObject);
        _animator = Get<Animator>.From(gameObject);
        _canvasGroup = Get<CanvasGroup>.From(gameObject);
        _mainCamera = Camera.main;
        _turnController = FindObjectOfType<TurnController>();
        _gameplayAnnouncer = FindObjectOfType<GameplayAnnouncer>();
    }

    private void OnEnable()
    {
        _turnController.OnTurnChanged += OnTurnChanged;
        _gameplayAnnouncer.OnGameStartAnnouncement += delegate { _hasGameStartBeenAnnounced = true; };
    }

    private void OnDisable()
    {
        _turnController.OnTurnChanged -= OnTurnChanged;
        _gameplayAnnouncer.OnGameStartAnnouncement -= delegate { _hasGameStartBeenAnnounced = true; };
    }

    protected virtual void Update()
    {
        _ray = CameraPoint.ScreenPointToRay(_mainCamera, Input.mousePosition);

        if (Physics.Raycast(_ray, out _raycastHit) && Input.GetMouseButton(0) && _canvasGroup.interactable)
        {
            _rectTransform.anchoredPosition = Input.mousePosition / _canvasRectransform.localScale.x;
            _isPlayingAnimation = true;
        }
        else
        {
            _isPlayingAnimation = false;
        }

        _animator.SetBool("isPlaying", _isPlayingAnimation);
    }

    public virtual void RemoteControlTargetActivity(bool isActive)
    {
        GlobalFunctions.CanvasGroupActivity(_canvasGroup, isActive);
        GlobalFunctions.CanvasGroupActivity(_mainTabCanvasGroup, !isActive && _hasGameStartBeenAnnounced);
        OnRemoteControlTargetActivity?.Invoke(isActive);
    }

    public void OnAnimationEnd()
    {
        OnSet?.Invoke(_raycastHit.point);
        RemoteControlTargetActivity(false);
    }

    private void OnTurnChanged(TurnState turnState)
    {
        RemoteControlTargetActivity(false);
    }
}
