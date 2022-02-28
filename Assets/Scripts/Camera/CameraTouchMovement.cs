using UnityEngine;
using UnityEngine.EventSystems;

public class CameraTouchMovement : MonoBehaviour
{
    private delegate bool Checker();
    private delegate float Value();

    private Checker _isMousePressed;
    private Checker _isMovingMouse;
    private Checker _isPointerOnUI;

    private Value _mouseX, _mouseY;

    public Vector3 TouchPosition;


    private void Awake()
    {
        _mouseX = delegate { return Input.GetAxis("Mouse X"); };
        _mouseY = delegate { return Input.GetAxis("Mouse Y"); };

        _isMousePressed = delegate { return Input.GetMouseButton(0); };
        _isMovingMouse = delegate { return _mouseX() != 0 || _mouseY() != 0; };
        _isPointerOnUI = delegate { return EventSystem.current.IsPointerOverGameObject(); };
    }

    private void Update()
    {
        Conditions<bool>.Compare(_isMousePressed() && _isMovingMouse() && !_isPointerOnUI(), OnMouseMovement, OnStopMouseMovement);
    }

    private void OnMouseMovement()
    {
        TouchPosition = new Vector3(_mouseX(), _mouseY(), 0);
    }

    private void OnStopMouseMovement()
    {
        TouchPosition = Vector3.zero;
    }
}
