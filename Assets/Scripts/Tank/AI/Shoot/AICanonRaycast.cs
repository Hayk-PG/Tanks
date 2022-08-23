using System;
using UnityEngine;

public class AICanonRaycast : MonoBehaviour
{
    private float _maxDistance = 10;
    private bool _isRaycastHit;
    private Transform _player;

    public Action<bool, float> OnAICanonRaycast { get; set; }

    private void Awake()
    {
        _player = GlobalFunctions.ObjectsOfType<TankController>.Find(tank => tank.BasePlayer != null).transform;
    }

    private void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * _maxDistance, Color.red);

        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, _maxDistance) && _player != null)
            _isRaycastHit = true;

        else
            _isRaycastHit = false;

        OnAICanonRaycast?.Invoke(_isRaycastHit, Vector3.Distance(hit.point, transform.position));
    }
}
