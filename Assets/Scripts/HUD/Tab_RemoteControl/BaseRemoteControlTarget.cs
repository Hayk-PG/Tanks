using System;
using System.Collections;
using UnityEngine;


public class BaseRemoteControlTarget : MonoBehaviour, IHudTabsObserver
{
    public enum Mode { None, Bomber, Artillery}
    protected Mode _mode;

    [SerializeField] [Space]
    protected RectTransform _canvas, _targetIcon;

    [SerializeField] [Space]
    protected CanvasGroup _canvasGroup;

    [SerializeField] [Space]
    protected Animator _animator;

    [SerializeField] [Space]
    protected Camera _mainCamera;

    [SerializeField] [Space]
    protected NotificationPanelManager _notificationPanelManager;

    protected Ray _ray;

    protected bool _isPlayingAnimation;

    protected object[] _data;

    protected string _artilleryNotificationPanelText = "To target your artillery, touch and hold on the screen where you want to aim!";
    protected string _airBomberNotificationPanelText = "To target your air bomber, touch and hold on the screen where you want to aim";

    public bool IsActive => _canvasGroup.interactable;

    public event Action<Mode, object[]> onRemoteControlTarget;




    private void OnEnable() => DropBoxSelectionHandler.onItemSelect += OnItemSelect;

    private void OnDisable() => DropBoxSelectionHandler.onItemSelect -= OnItemSelect;

    protected virtual void Update()
    {
        ControlTargetIcon();

        PlayAnimation();
    }

    private void OnItemSelect(DropBoxItemType dropBoxItemType, object[] data)
    {
        if (dropBoxItemType == DropBoxItemType.Bomber)
            OnSelectMode(Mode.Bomber, data);

        if (dropBoxItemType == DropBoxItemType.Artillery)
            OnSelectMode(Mode.Artillery, data);
    }

    private void OnSelectMode(Mode mode, object[] data)
    {
        _data = data;

        SetMode(mode);

        SetActivity(true);
    }

    private void SetMode(Mode mode) => _mode = mode;

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

        GameSceneObjectsReferences.HudTabsHandler.RequestTabActivityPermission(this, HudTabsHandler.HudTab.TabRemoteControl, isActive);
    }

    public void Execute(bool isActive)
    {
        GlobalFunctions.CanvasGroupActivity(_canvasGroup, isActive);

        LockCamera(isActive);

        StartCoroutine(StartCloseTabTimer(isActive));

        ChangeCameraHalfLength(isActive);

        SetNotificationPanelActive(isActive);
    }

    private void LockCamera(bool isActive)
    {
        GameSceneObjectsReferences.MainCameraController.CameraOffset(null, null, null, 1, MainCameraController.CameraUser.RemoteControl, isActive);
    }

    private void ChangeCameraHalfLength(bool isActive) => GameSceneObjectsReferences.MainCameraController.ChangeHalfLength(new Vector3(1.5f, 0, 0), !isActive);

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

    private void SetNotificationPanelActive(bool isActive)
    {
        if (!isActive)
        {
            _notificationPanelManager.Set();

            return;
        }

        _notificationPanelManager.Set(NotificationPanelManager.InteractionType.Close, null, _mode == Mode.Artillery ? _artilleryNotificationPanelText : _mode == Mode.Bomber ?
                                      _airBomberNotificationPanelText : "", "Ok!");
    }

    public void OnAnimationEnd()
    {
        RaiseBomberTargetEvent();

        SetActivity(false);
    }

    private void RaiseBomberTargetEvent()
    {
        _data[3] = (_ray.direction + _ray.origin);

        if (_mode == Mode.None)
            return;

        onRemoteControlTarget?.Invoke(_mode, _data);
    }
}
