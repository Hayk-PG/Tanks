using UnityEngine;
using UnityEngine.EventSystems;

public class CameraTouchMovement : MonoBehaviour
{
    private Camera _mainCamera;
    private BaseRemoteControlTarget[] _baseRemoteControlTargets;

    private bool _canCameraMove = true;
    private float _cameraMovementResetTimer;
    private int _fingerId;

    public Vector3 TouchPosition { get; set; }
    public bool IsCameraMoving { get; set; }
    



    private void Awake()
    {
        _mainCamera = Get<Camera>.From(gameObject);
        _baseRemoteControlTargets = FindObjectsOfType<BaseRemoteControlTarget>();
    }

    private void OnEnable()
    {
        GlobalFunctions.Loop<BaseRemoteControlTarget>.Foreach(_baseRemoteControlTargets, rcar => { rcar.OnRemoteControlTargetActivity += OnRemoteControlTargetActivity; });
    }

    private void OnDisable()
    {
        GlobalFunctions.Loop<BaseRemoteControlTarget>.Foreach(_baseRemoteControlTargets, rcar => { rcar.OnRemoteControlTargetActivity -= OnRemoteControlTargetActivity; });
    }

    private void Update()
    {
        TouchMovement();
    }

    private bool IsPointerOnUI()
    {
        if (!PlatformChecker.IsEditor)
        {
            foreach (var touch in Input.touches)
            {
                _fingerId = touch.fingerId;
            }

            return EventSystem.current.IsPointerOverGameObject(_fingerId);
        }
        else
        {
            return EventSystem.current.IsPointerOverGameObject();
        }
    }

    private Vector3 MousePosition()
    {
        return CameraPoint.WorldPoint(_mainCamera, Input.mousePosition);
    }

    private void TouchMovement()
    {
        Conditions<bool>.Compare(Input.GetMouseButton(0) && _canCameraMove && !IsPointerOnUI(), OnMouseMovement, OnStopMouseMovement);
    }

    private void OnMouseMovement()
    {
        IsCameraMoving = true;
        TouchPosition = MousePosition();
    }

    private void OnStopMouseMovement()
    {
        if (IsCameraMoving)
        {
            Stop();
        }
    }

    private void Stop()
    {
        Conditions<bool>.Compare(Input.GetMouseButton(0), ()=> _cameraMovementResetTimer = 0, () => _cameraMovementResetTimer += Time.deltaTime);
        Conditions<float>.Compare(_cameraMovementResetTimer, 3, null, ResetTouchMovementTimer, null);
    }

    private void ResetTouchMovementTimer()
    {
        IsCameraMoving = false;
        _cameraMovementResetTimer = 0;
    }

    private void OnRemoteControlTargetActivity(bool isActive)
    {
        _canCameraMove = !isActive;
    }
}
