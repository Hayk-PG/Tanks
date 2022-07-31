using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICanonRaycast : MonoBehaviour
{
    private float _maxDistance = 10;
    private float _distance;
    private bool _isRaycastHit;
    private Transform _player;

    public Action<float, bool> OnAICanonRaycast { get; set; }

    private void Awake()
    {
        _player = GlobalFunctions.ObjectsOfType<TankController>.Find(tank => tank.BasePlayer != null).transform;
    }

    private void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * _maxDistance, Color.red);

        if(Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, _maxDistance) && _player != null)
        {
            _distance = hit.transform.position.x + -_player.position.x;
            _isRaycastHit = true;
        }
        else
        {
            _isRaycastHit = false;           
        }

        OnAICanonRaycast?.Invoke(_distance, _isRaycastHit);
    }
}
