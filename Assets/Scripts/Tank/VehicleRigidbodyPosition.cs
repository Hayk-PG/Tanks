using UnityEngine;

public class VehicleRigidbodyPosition : MonoBehaviour
{
    private BaseTankMovement _baseTankMovement;

    private float _xPositionMinLimit = -5.1f;
    private float _xPositionMaxLimit = 5.1f;

    private delegate bool Checker(Rigidbody rigidBody);
    private Checker _isLesserThanMinLimit;
    private Checker _isGreaterThanMaxLimit;
    private Checker _isAtAllowedPosition;


    private void Awake()
    {
        _baseTankMovement = Get<BaseTankMovement>.From(gameObject);

        _isLesserThanMinLimit = delegate (Rigidbody rb) { return rb.position.x <= _xPositionMinLimit; };
        _isGreaterThanMaxLimit = delegate (Rigidbody rb) { return rb.position.x >= _xPositionMaxLimit; };
        _isAtAllowedPosition = delegate (Rigidbody rb) { return rb.position.x > _xPositionMinLimit && rb.position.x < _xPositionMaxLimit; };
    }

    private void OnEnable()
    {
        if (_baseTankMovement != null) _baseTankMovement.OnRigidbodyPosition += OnRigidbodyPosition;
    }

    private void OnDisable()
    {
        if (_baseTankMovement != null) _baseTankMovement.OnRigidbodyPosition -= OnRigidbodyPosition;
    }

    private void OnRigidbodyPosition(Rigidbody rigidBody)
    {
        Conditions<bool>.Compare(_isLesserThanMinLimit(rigidBody), () => XPositionLesser(rigidBody), null);
        Conditions<bool>.Compare(_isGreaterThanMaxLimit(rigidBody), () => XPositionGreater(rigidBody), null);
        Conditions<bool>.Compare(_isAtAllowedPosition(rigidBody), () => AtAllowedPosition(rigidBody), null);
    }

    private void XPositionLesser(Rigidbody rigidBody)
    {
        rigidBody.position = new Vector3(_xPositionMinLimit, rigidBody.position.y, 0);
    }

    private void XPositionGreater(Rigidbody rigidBody)
    {
        rigidBody.position = new Vector3(_xPositionMaxLimit, rigidBody.position.y, 0);
    }

    private void AtAllowedPosition(Rigidbody rigidBody)
    {
        rigidBody.position = new Vector3(rigidBody.transform.position.x, rigidBody.transform.position.y, 0);
    }
}
