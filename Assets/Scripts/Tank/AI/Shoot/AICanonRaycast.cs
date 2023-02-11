using System;
using UnityEngine;

public class AICanonRaycast : MonoBehaviour
{
    private float _maxDistance = 10;

    private bool _isRaycastHit;

    private Transform _player;

    public event Action<bool, float> onRayCast;




    private void Awake()
    {
        _player = GlobalFunctions.ObjectsOfType<TankController>.Find(tank => tank.BasePlayer != null).transform;
    }

    private void FixedUpdate()
    {
        Debug.DrawRay(transform.position, transform.forward * _maxDistance, Color.red);

        CastRay(out RaycastHit hit);

        RaiseRayCastEvent(hit);
    }

    private void CastRay(out RaycastHit hit)
    {
        if (Physics.Raycast(transform.position, transform.forward, out hit, _maxDistance) && _player != null)
            _isRaycastHit = true;

        else
            _isRaycastHit = false;
    }

    private void RaiseRayCastEvent(RaycastHit hit)
    {
        onRayCast?.Invoke(_isRaycastHit, Vector3.Distance(hit.point, transform.position));
    }
}
