using UnityEngine;
using System.Linq;
using UnityEngine.EventSystems;

public class CameraTouchMovement : MonoBehaviour
{
    private delegate bool Checker();
    private delegate float Value();

    private Checker _isMousePressed;
    private Checker _isMovingMouse;
    private Checker _isPointerOnUI;

    private Value _mouseX, _mouseY;

    public Vector3 TouchPosition { get; set; }
    public bool IsCameraMoving { get; set; }

    private float _cameraMovementResetTimer;

    private UITouchMovement _uiTouchMovement;



    private void Awake()
    {
        _uiTouchMovement = FindObjectOfType<UITouchMovement>();

        _mouseX = delegate { return Input.GetAxis("Mouse X"); };
        _mouseY = delegate { return Input.GetAxis("Mouse Y"); };

        _isMousePressed = delegate { return Input.GetMouseButton(0); };
        _isMovingMouse = delegate { return _mouseX() != 0 || _mouseY() != 0; };
        _isPointerOnUI = delegate { return EventSystem.current.IsPointerOverGameObject(); };
    }

    private void OnEnable()
    {
        if (_uiTouchMovement != null) _uiTouchMovement.OnCameraCanMove += TouchMovement;
    }

    private void OnDisable()
    {
        if (_uiTouchMovement != null) _uiTouchMovement.OnCameraCanMove -= TouchMovement;
    }

    private void TouchMovement()
    {
        Conditions<bool>.Compare(_isMousePressed() && _isMovingMouse() && !_isPointerOnUI(), OnMouseMovement, OnStopMouseMovement);
    }

    private void OnMouseMovement()
    {
        IsCameraMoving = true;
        TouchPosition = new Vector3(Mathf.Clamp(_mouseX(), -1, 1), Mathf.Clamp(_mouseY(), -1, 1), 0); 
    }

    private void OnStopMouseMovement()
    {
        Conditions<bool>.Compare(IsCameraMoving, ResetCameraMovement, null);

        TouchPosition = Vector3.zero;
    }

    private void ResetCameraMovement()
    {
        _cameraMovementResetTimer += Time.deltaTime;

        Conditions<float>.Compare(_cameraMovementResetTimer, 1, null, 
            delegate 
            {
                IsCameraMoving = false;
                _cameraMovementResetTimer = 0;
            }, null);
    }
}
