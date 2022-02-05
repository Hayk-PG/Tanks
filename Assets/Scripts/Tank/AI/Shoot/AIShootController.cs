using System;
using UnityEngine;

public class AIShootController : MonoBehaviour
{
    [Serializable] struct Canon
    {
        [SerializeField] internal Transform _canonPivotPoint;
        [SerializeField] internal float _x, _y, _z;
    }
    [Serializable] struct Shoot
    {
        [SerializeField] internal AIShootTrajectory _aIShootTrajectory;

        [Header("Bullet")]
        [SerializeField] internal BulletController[] _bulletsPrefab;
        [SerializeField] internal Transform _shootPoint;
        [SerializeField] internal int _activeBulletIndex;
        [SerializeField] internal bool _canShoot;
    }

    [SerializeField] Canon _canon;
    [SerializeField] Shoot _shoot;

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
            _target = _shoot._aIShootTrajectory.PredictedTrajectory(_player.position, transform.position, 1);
        }

        RotateCanon();

        if (_shoot._canShoot)
        {
            ShootBullet();
            _shoot._canShoot = false;
        }
    }

    public void RotateCanon()
    {
        Quaternion lookRot = Quaternion.LookRotation(Vector3.forward, _target);
        Quaternion rot = lookRot * Quaternion.Euler(_canon._x, _canon._y, _canon._z);

        float deltaTime = 2 * Time.deltaTime;

        _canon._canonPivotPoint.rotation = Quaternion.Slerp(_canon._canonPivotPoint.rotation, rot, deltaTime);
    }

    void ShootBullet()
    {
        BulletController bullet = Instantiate(_shoot._bulletsPrefab[_shoot._activeBulletIndex], _shoot._shootPoint.position, _canon._canonPivotPoint.rotation);
        bullet.RigidBody.velocity = _target;
    }
}
