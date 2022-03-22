using UnityEngine;
using System.Linq;
using UnityEngine.EventSystems;

public class CameraTouchMovement : MonoBehaviour
{
    private delegate bool Checker();
    private delegate float Value();

    private Checker _touchPhaseMoved;
    private Checker __touchInputReleased;

    private Value _mouseX, _mouseY;

    public Vector3 TouchPosition { get; set; }
    public bool IsCameraMoving { get; set; }

    private float _cameraMovementResetTimer;



    private void Awake()
    {
        _mouseX = delegate { return Input.GetAxis("Mouse X") * 3 * Time.deltaTime; };
        _mouseY = delegate { return Input.GetAxis("Mouse Y") * 3 * Time.deltaTime; };

        _touchPhaseMoved = delegate 
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
                return !EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
            else
                return false;
        };

        __touchInputReleased = delegate
        {
            return Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended;
        };
    }

    private void Update()
    {
        TouchMovement();
    }

    private void TouchMovement()
    {
        Conditions<bool>.Compare(_touchPhaseMoved(), OnMouseMovement, OnStopMouseMovement);
    }

    private void OnMouseMovement()
    {
        IsCameraMoving = true;
        TouchPosition = new Vector3(Mathf.Clamp(_mouseX(), -1, 1), Mathf.Clamp(_mouseY(), -1, 1), 0); 
    }

    private void OnStopMouseMovement()
    {
        if (IsCameraMoving)
        {
            Stop();
        }

        TouchPosition = Vector3.zero;
    }

    private void Stop()
    {
        Conditions<bool>.Compare(__touchInputReleased(), ()=> _cameraMovementResetTimer = 0, () => _cameraMovementResetTimer += Time.deltaTime);
        Conditions<float>.Compare(_cameraMovementResetTimer, 1, null, ResetTouchMovementTimer, null);
    }

    private void ResetTouchMovementTimer()
    {
        IsCameraMoving = false;
        _cameraMovementResetTimer = 0;
    }
}
