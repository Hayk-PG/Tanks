using System;
using UnityEngine;

public class BaseShootController: MonoBehaviour
{
    public Transform _canonPivotPoint;
    public Transform _shootPoint;
    public BaseTrajectory _trajectory;

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

        [Header("Force")]
        [SerializeField] internal float _minForce;
        [SerializeField] internal float _maxForce;
        [SerializeField] internal float _smoothTime;
        [SerializeField] internal float _maxSpeed;
        internal float _currentVelocity;
        internal bool _isApplyingForce;
    }

    public Canon _canon;
    public Shoot _shoot;
}
