using System;
using UnityEngine;

public class AIShootController : MonoBehaviour
{
    [Serializable] struct Canon
    {
        [SerializeField] internal Transform _canonPivotPoint;
        [SerializeField] internal float _x, _y, _z;
    }

    [SerializeField] Canon _canon;
    [SerializeField] AIShootTrajectory _aIShootTrajectory;

    Transform _player;
    Vector3 _target;


    void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (_player != null)
        {
            _target = _aIShootTrajectory.PredictedTrajectory(_player.position, transform.position, 1);
        }

        RotateCanon();
    }

    public void RotateCanon()
    {
        Quaternion lookRot = Quaternion.LookRotation(Vector3.forward, _target);
        Quaternion rot = lookRot * Quaternion.Euler(_canon._x, _canon._y, _canon._z);

        float deltaTime = 2 * Time.deltaTime;

        _canon._canonPivotPoint.rotation = Quaternion.Slerp(_canon._canonPivotPoint.rotation, rot, deltaTime);
    }
}
