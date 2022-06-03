using UnityEngine;

public class VehicleRigidbodyPosition : MonoBehaviour
{
    private BaseTankMovement _baseTankMovement;
    private LevelGenerator _levelGenerator;

    private float _xPositionMinLimit;
    private float _xPositionMaxLimit;

    private delegate bool Checker(Rigidbody rigidBody);
    private Checker _isLesserThanMinLimit;
    private Checker _isGreaterThanMaxLimit;
    private Checker _isAtAllowedPosition;


    private void Awake()
    {
        _baseTankMovement = Get<BaseTankMovement>.From(gameObject);
        _levelGenerator = FindObjectOfType<LevelGenerator>();

        _xPositionMinLimit = _levelGenerator.MapHorizontalStartPoint;
        _xPositionMaxLimit = _levelGenerator.MapHorizontalEndPoint;
        print(_xPositionMinLimit + "/" + _xPositionMaxLimit);

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
        rigidBody.position = new Vector3(rigidBody.position.x, rigidBody.position.y, 0);
    }
}
