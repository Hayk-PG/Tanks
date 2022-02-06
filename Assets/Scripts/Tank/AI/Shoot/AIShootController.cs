using System;
using System.Collections;
using UnityEngine;

public class AIShootController : MonoBehaviour
{
    [Serializable]
    private struct Canon
    {
        [SerializeField] internal Transform _canonPivotPoint;
        [SerializeField] internal float _x, _y, _z;
    }
    [Serializable]
    private struct Shoot
    {
        [SerializeField] internal AIShootTrajectory _aIShootTrajectory;

        [Header("Bullet")]
        [SerializeField] internal BulletController[] _bulletsPrefab;
        [SerializeField] internal Transform _shootPoint;
        [SerializeField] internal int _activeBulletIndex;
    }

    [SerializeField]
    private Canon _canon;
    [SerializeField]
    private Shoot _shoot;

    private Transform _player;
    private Vector3 _target;
    private Rigidbody _rigidBody;
    private PlayerTurn _playerTurn;
    private AITankMovement _aiTankMovement;


    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _rigidBody = GetComponent<Rigidbody>();
        _playerTurn = GetComponent<PlayerTurn>();
        _aiTankMovement = GetComponent<AITankMovement>();
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
        if (_player != null)
        {
            _target = _shoot._aIShootTrajectory.PredictedTrajectory(_player.position, transform.position, 1);
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

    private void ShootBullet()
    {
        StartCoroutine(ShootBulletCoroutine());
    }

    private IEnumerator ShootBulletCoroutine()
    {
        yield return new WaitForSeconds(2);

        BulletController bullet = Instantiate(_shoot._bulletsPrefab[_shoot._activeBulletIndex], _shoot._shootPoint.position, _canon._canonPivotPoint.rotation);
        bullet.RigidBody.velocity = _target;
        _rigidBody.AddForce(transform.forward * _target.magnitude * 1000, ForceMode.Impulse);
    }
}
