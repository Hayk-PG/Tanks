using System;
using System.Collections;
using UnityEngine;

public class AIShootController : MonoBehaviour
{
    [Serializable]
    private struct Canon
    {
        [SerializeField]
        internal Transform _canonPivotPoint;
        [SerializeField]
        internal float _x, _y, _z;
        internal float _t;

        internal Quaternion _lookRot, _rot, _desiredRotation;
    }
    [Serializable]
    private struct Shoot
    {
        [SerializeField]
        internal AIShootTrajectory _aIShootTrajectory;

        [Header("Bullet")]
        [SerializeField]
        internal BulletController[] _bulletsPrefab;
        [SerializeField]
        internal Transform _shootPoint;
        [SerializeField]
        internal int _activeBulletIndex;
    }

    [SerializeField]
    private Canon _canon;
    [SerializeField]
    private Shoot _shoot;

    private Transform _player;
    private Vector3 _target;
    private Rigidbody _rigidBody;
    private AITankMovement _aiTankMovement;
    private IScore _iScore;

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _rigidBody = GetComponent<Rigidbody>();
        _aiTankMovement = GetComponent<AITankMovement>();
        _iScore = Get<IScore>.From(gameObject);
    }

    private void OnEnable()
    {
        _aiTankMovement.Shoot += ShootBullet;   
    }

    private void OnDisable()
    {
        _aiTankMovement.Shoot -= ShootBullet;
    }

    private void Update()
    {
        if (_player != null) _target = _shoot._aIShootTrajectory.PredictedTrajectory(_player.position, transform.position, 1);

        RotateCanon();
    }

    public void RotateCanon()
    {
        _canon._lookRot = Quaternion.LookRotation(Vector3.forward, _target);
        _canon._rot = _canon._lookRot * Quaternion.Euler(_canon._x, _canon._y, _canon._z);
        _canon._desiredRotation = Quaternion.Slerp(_canon._desiredRotation, _canon._rot, _canon._t);

        _canon._t = 2 * Time.deltaTime;

        _canon._canonPivotPoint.rotation = _canon._desiredRotation;
    }

    private void ShootBullet()
    {
        StartCoroutine(ShootBulletCoroutine());
    }

    private IEnumerator ShootBulletCoroutine()
    {
        yield return new WaitForSeconds(2);

        BulletController bullet = Instantiate(_shoot._bulletsPrefab[_shoot._activeBulletIndex], _shoot._shootPoint.position, _canon._canonPivotPoint.rotation);
        bullet.OwnerScore = _iScore;
        bullet.RigidBody.velocity = _target;
        _rigidBody.AddForce(transform.forward * _target.magnitude * 1000, ForceMode.Impulse);
    }
}
