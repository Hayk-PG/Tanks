using System;
using UnityEngine;

public class BaseShootController: MonoBehaviour
{
    protected Transform _canonPivotPoint;
    protected Transform _shootPoint;
    protected BaseTrajectory _trajectory;

    [Serializable] public struct Canon
    {
        internal float _currentEulerAngleX;

        [Header("Canon rotation parameters")]
        public float _minEulerAngleX;
        public float _maxEulerAngleX;
        public float _rotationSpeed;
        public Vector3 _rotationStabilizer;
    }

    [Serializable] public struct Shoot
    {
        internal float _currentForce;

        [Header("Shoot force")]
        public float _minForce;
        public float _maxForce;
        public float _smoothTime;
        public float _maxSpeed;
        internal float _currentVelocity;
        internal bool _isApplyingForce;
    }

    public Canon _canon;
    public Shoot _shoot;


    protected virtual void Awake()
    {
        _canonPivotPoint = transform.Find("CanonPivotPoint");
        _shootPoint = Get<BaseTrajectory>.FromChild(_canonPivotPoint.gameObject).transform;
        _trajectory = Get<BaseTrajectory>.From(_shootPoint.gameObject);
    }
}
