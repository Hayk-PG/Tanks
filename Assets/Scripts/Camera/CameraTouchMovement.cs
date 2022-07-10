using UnityEngine;
using UnityEngine.EventSystems;

public class CameraTouchMovement : MonoBehaviour
{
    private Camera _mainCamera;

    private delegate bool Checker();
    private delegate float Value();

    private Checker _touchPhaseMoved;
    private Checker _touchInputReleased;

    private int _fingerId;

    public Vector3 TouchPosition { get; set; }
    public bool IsCameraMoving { get; set; }
    private float _cameraMovementResetTimer;



    private void Awake()
    {
        _mainCamera = Get<Camera>.From(gameObject);

        _touchPhaseMoved = delegate 
        {
            if (Input.GetMouseButton(0))
                return !EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
            else
                return false;
        };
        _touchInputReleased = delegate
        {
            return !Input.GetMouseButton(0);
        };
    }

    private void Update()
    {
        TouchMovement();
    }

    private bool IsPointerOnUI()
    {
        foreach (var touch in Input.touches)
        {
            _fingerId = touch.fingerId;
        }

        return EventSystem.current.IsPointerOverGameObject(_fingerId);
    }

    private Vector3 MousePosition()
    {
        return CameraPoint.WorldPoint(_mainCamera, Input.mousePosition);
    }

    private void TouchMovement()
    {
        Conditions<bool>.Compare(Input.GetMouseButton(0), OnMouseMovement, OnStopMouseMovement);
    }

    private void OnMouseMovement()
    {
        if (PlatformChecker.IsEditor || !PlatformChecker.IsEditor && !IsPointerOnUI())
        {
            IsCameraMoving = true;
            TouchPosition = MousePosition();
        }

        if (EventSystem.current.IsPointerOverGameObject())
        {
            print("IsPointerOnUI");
        }
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
        Conditions<bool>.Compare(!_touchInputReleased(), ()=> _cameraMovementResetTimer = 0, () => _cameraMovementResetTimer += Time.deltaTime);
        Conditions<float>.Compare(_cameraMovementResetTimer, 3, null, ResetTouchMovementTimer, null);
    }

    private void ResetTouchMovementTimer()
    {
        IsCameraMoving = false;
        _cameraMovementResetTimer = 0;
    }
}
