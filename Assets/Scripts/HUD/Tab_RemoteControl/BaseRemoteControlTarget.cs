﻿using System;
using System.Collections;
using UnityEngine;


public class BaseRemoteControlTarget : MonoBehaviour
{
    [SerializeField] [Space]
    protected RectTransform _canvas, _targetIcon;

    [SerializeField] [Space]
    protected CanvasGroup _canvasGroup;

    [SerializeField] [Space]
    protected Animator _animator;

    [SerializeField] [Space]
    protected Camera _mainCamera;

    protected Ray _ray;

    protected bool _isPlayingAnimation;

    private object[] _data = new object[4];

    public bool IsActive => _canvasGroup.interactable;

    public event Action<object[]> onBomberTargetSet;

    public event Action<bool> onRemoteControlActivity;

   



    private void OnEnable()
    {
        GlobalFunctions.Loop<DropBoxSelectionPanelBomber>.Foreach(GameSceneObjectsReferences.DropBoxSelectionPanelBombers, selectedBobmer => { selectedBobmer.onCallBomber += OnSelectBomber; });
    }

    private void OnDisable()
    {
        GlobalFunctions.Loop<DropBoxSelectionPanelBomber>.Foreach(GameSceneObjectsReferences.DropBoxSelectionPanelBombers, selectedBobmer => { selectedBobmer.onCallBomber -= OnSelectBomber; });
    }

    protected virtual void Update()
    {
        ControlTargetIcon();

        PlayAnimation();
    }

    private void OnSelectBomber(BomberType bomberType, int price, int quantity)
    {
        _data[0] = bomberType;
        _data[1] = price;
        _data[2] = quantity;

        SetActivity(true);
    }

    private void ControlTargetIcon()
    {
        if (!Input.GetMouseButton(0) || !_canvasGroup.interactable)
        {
            _isPlayingAnimation = false;

            return;
        }

        _ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

        _targetIcon.position = _mainCamera.WorldToScreenPoint(_ray.direction + _ray.origin);

        _isPlayingAnimation = true;
    }

    private void PlayAnimation() => _animator.SetBool("isPlaying", _isPlayingAnimation);

    protected virtual void SetActivity(bool isActive)
    {
        if (_canvasGroup.interactable == isActive)
            return;

        GlobalFunctions.CanvasGroupActivity(_canvasGroup, isActive);

        LockCamera(isActive);

        StartCoroutine(StartCloseTabTimer(isActive));

        onRemoteControlActivity?.Invoke(isActive);
    }

    private void LockCamera(bool isActive)
    {
        GameSceneObjectsReferences.MainCameraController.CameraOffset(null, null, null, 1, MainCameraController.CameraUser.RemoteControl, isActive);
    }

    private IEnumerator StartCloseTabTimer(bool isActive)
    {
        if (isActive)
        {
            int seconds = 0;

            while (_canvasGroup.interactable)
            {
                seconds++;

                if (seconds >= 15)
                    SetActivity(false);

                yield return new WaitForSeconds(1);
            }
        }
    }

    public void OnAnimationEnd()
    {
        RaiseBomberTargetEvent();

        SetActivity(false);
    }

    private void RaiseBomberTargetEvent()
    {
        _data[3] = (_ray.direction + _ray.origin);

        onBomberTargetSet?.Invoke(_data);
    }
}
