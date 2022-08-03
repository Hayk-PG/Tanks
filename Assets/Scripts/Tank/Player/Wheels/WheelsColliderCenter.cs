using System.Collections;
using UnityEngine;

public class WheelsColliderCenter : MonoBehaviour
{
    private WheelCollider[] _wheelColliders;
    private TankMovement _tankMovement;
    private bool _isTankMoving;
    private bool _isTankStopped;
    private bool _isCoroutineStarted;
    private bool _isCoroutineFinished;
    private Vector3 _currentCenter;
    private Vector3 _default;
    private Vector3 _low;


    private void Awake()
    {
        _wheelColliders = GetComponentsInChildren<WheelCollider>();
        _tankMovement = Get<TankMovement>.From(gameObject);

        _default = _wheelColliders[0].center;
        _currentCenter = _default;
        _low = Vector3.zero;
    }

    private void OnEnable()
    {
        _tankMovement.OnDirectionValue += OnDirectionValue;
    }

    private void OnDisable()
    {
        _tankMovement.OnDirectionValue -= OnDirectionValue;
    }

    private void OnDirectionValue(float direction)
    {
        _isTankMoving = direction >= 1 || direction <= -1;
        _isTankStopped = direction == 0;

        if (_isTankMoving && !_isCoroutineStarted && !_isCoroutineFinished)
            StartCoroutine(Coroutine());

        if (_isTankStopped && _isCoroutineFinished)
        {
            _isCoroutineStarted = false;
            _isCoroutineFinished = false;
        }     
    }

    private void ModifyWheelsCenter(Vector3 target)
    {
        GlobalFunctions.Loop<WheelCollider>.Foreach(_wheelColliders, wheel =>
        {
            wheel.center = Vector3.LerpUnclamped(wheel.center, target, 150 * Time.deltaTime);
            _currentCenter = wheel.center;
        });
    }

    private IEnumerator Coroutine()
    {
        _isCoroutineStarted = true;
        _isCoroutineFinished = false;

        while (_currentCenter.y > _low.y)
        {
            ModifyWheelsCenter(_low);
            yield return null;
        }

        yield return null;
        _currentCenter = _low;

        while (_currentCenter.y < _default.y)
        {
            ModifyWheelsCenter(_default);
            yield return null;
        }

        yield return null;

        _currentCenter = _default;
        _isCoroutineFinished = true;
    }
}
