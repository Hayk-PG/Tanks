using System;
using UnityEngine;

public class BaseRemoteControlTarget : MonoBehaviour
{
    [SerializeField] protected RectTransform _canvasRectransform;
    protected RectTransform _rectTransform;
    protected Animator _animator;
    protected CanvasGroup _canvasGroup;
    protected Ray _ray;
    protected RaycastHit _raycastHit;
    protected Camera _mainCamera;
    protected bool _isPlayingAnimation;

    public Action<bool> OnRemoteControlTargetActivity { get; set; }
    public Action<Vector3> OnSet { get; set; }



    protected virtual void Awake()
    {
        _rectTransform = Get<RectTransform>.From(gameObject);
        _animator = Get<Animator>.From(gameObject);
        _canvasGroup = Get<CanvasGroup>.From(gameObject);
        _mainCamera = Camera.main;      
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
        OnRemoteControlTargetActivity?.Invoke(isActive);
    }

    public void OnAnimationEnd()
    {
        OnSet?.Invoke(_raycastHit.point);
    }
}
