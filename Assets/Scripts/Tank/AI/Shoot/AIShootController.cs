using System.Collections;
using UnityEngine;

public class AIShootController : BaseShootController
{
    private Transform _player;
    private Vector3 _target;
    private Rigidbody _rigidBody;
    private AITankMovement _aiTankMovement;
    private IScore _iScore;
    private Quaternion _lookRot;
    private Quaternion _rot;
    private Quaternion _desiredRotation;

    [SerializeField] private BulletController[] _bulletsPrefab;
    [SerializeField] private int _activeBulletIndex;
    private bool CanRotateCanon
    {
        get
        {
            return Converter.AngleConverter(_canon._currentEulerAngleX) < _canon._maxEulerAngleX &&
                   Converter.AngleConverter(_canon._currentEulerAngleX) > _canon._minEulerAngleX;
        }
    }


    protected override void Awake()
    {
        base.Awake();
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
        RotateCanon();
    }

    public void RotateCanon()
    {
        if (CanRotateCanon)
        {
            _target = _trajectory.PredictedTrajectory(_player.position, transform.position, 1);           
        }

        _lookRot = Quaternion.LookRotation(Vector3.forward, _target);
        _rot = _lookRot * Quaternion.Euler(_canon._rotationStabilizer.x, _canon._rotationStabilizer.y, _canon._rotationStabilizer.z);
        _desiredRotation = Quaternion.Slerp(_desiredRotation, _rot, _canon._rotationSpeed);
        _canon._rotationSpeed = 2 * Time.deltaTime;
        _canon._currentEulerAngleX = _desiredRotation.eulerAngles.x;
        _canonPivotPoint.rotation = _desiredRotation;
    }

    private void ShootBullet()
    {
        StartCoroutine(ShootBulletCoroutine());
    }

    private IEnumerator ShootBulletCoroutine()
    {
        yield return new WaitForSeconds(2);

        BulletController bullet = Instantiate(_bulletsPrefab[_activeBulletIndex], _shootPoint.position, _canonPivotPoint.rotation);
        bullet.OwnerScore = _iScore;
        bullet.RigidBody.velocity = _target;
        _rigidBody.AddForce(transform.forward * _target.magnitude * 1000, ForceMode.Impulse);
        mainCameraController.SetTarget(_playerTurn, bullet.transform);
    }
}
