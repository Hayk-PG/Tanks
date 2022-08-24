using System.Collections;
using UnityEngine;

public class AIShootController : BaseShootController
{
    [SerializeField] private BulletController[] _bulletsPrefab;
    [SerializeField] private int _activeBulletIndex;

    private Rigidbody _rigidBody;
    private AITankMovement _aiTankMovement;
    private AIEnemyDataGetter _aiEnemyDataGetter;
    private Vector3 _target;    
    private IScore _iScore;
    private Quaternion _lookRot;
    private Quaternion _rot;
    private Quaternion _desiredRotation;
     
    private float _defaultTrajectoryTime = 1;
    private float _desiredTrajectoryTime = 1;
    private float _currentTrajectoryTime = 1;
    private float _currentOffsetX = 0;

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
        _rigidBody = GetComponent<Rigidbody>();
        _aiTankMovement = GetComponent<AITankMovement>();
        _aiEnemyDataGetter = Get<AIEnemyDataGetter>.From(gameObject);
        _iScore = Get<IScore>.From(gameObject);
    }

    private void Start()
    {
        AIShootBehaviour(Data.Manager.SingleGameDifficultyLevel);
    }

    private void OnEnable()
    {
        _aiTankMovement.Shoot += ShootBullet;
    }

    private void OnDisable()
    {
        _aiTankMovement.Shoot -= ShootBullet;
        _aiCanonRaycast.OnAICanonRaycast -= OnAICanonRaycast;
    }

    private void Update()
    {
        RotateCanon();
    }

    private void SubscribeToAICanonRaycast()
    {
        if (_aiCanonRaycast != null)
            _aiCanonRaycast.OnAICanonRaycast += OnAICanonRaycast; 
    }

    private void AIShootBehaviour(SingleGameDifficultyLevel singleGameDifficultyLevel)
    {
        switch (singleGameDifficultyLevel)
        {
            case SingleGameDifficultyLevel.Easy: return;
            case SingleGameDifficultyLevel.Normal:
                SubscribeToAICanonRaycast();
                StartCoroutine(OnHigherThanEasyDifficultyLevel());         
                break;

            case SingleGameDifficultyLevel.Hard:
                SubscribeToAICanonRaycast();
                _desiredTrajectoryTime = 2;
                break;
        }
    }

    private IEnumerator OnHigherThanEasyDifficultyLevel()
    {
        while (true)
        {
            _desiredTrajectoryTime = Random.Range(1.5f, 2);
            yield return new WaitForSeconds(2);
        }
    }

    private void OnAICanonRaycast(bool isRaycastHit, float hitPointDistance)
    {
        if (CanRotateCanon)
        {
            if (isRaycastHit)
            {
                if (hitPointDistance > _aiEnemyDataGetter.Distance || hitPointDistance <= _aiEnemyDataGetter.Distance && _aiEnemyDataGetter.Distance > 8)
                {
                    _currentTrajectoryTime = _desiredTrajectoryTime;
                    _currentOffsetX = Mathf.Lerp(0, _aiEnemyDataGetter.Distance, _currentTrajectoryTime) / 10;
                }

                if (hitPointDistance <= _aiEnemyDataGetter.Distance && _aiEnemyDataGetter.Distance < 8)
                {
                    _currentTrajectoryTime = _defaultTrajectoryTime;
                    _currentOffsetX = 0;
                }
            }
            else
            {
                if (_aiEnemyDataGetter.Distance > 8)
                {
                    _currentTrajectoryTime = _desiredTrajectoryTime;
                    _currentOffsetX = Mathf.Lerp(0, _aiEnemyDataGetter.Distance, _currentTrajectoryTime) / 10;
                }
                else
                {
                    _currentTrajectoryTime = _defaultTrajectoryTime;
                    _currentOffsetX = 0;
                }
            }
        }
    }

    public void RotateCanon()
    {
        _target = _trajectory.PredictedTrajectory(new Vector3(_aiEnemyDataGetter.Enemy.position.x - _currentOffsetX, _aiEnemyDataGetter.Enemy.position.y, _aiEnemyDataGetter.Enemy.position.z), transform.position, _currentTrajectoryTime);
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
        mainCameraController.CameraOffset(_playerTurn, bullet.transform, null, null);
        _currentTrajectoryTime = _defaultTrajectoryTime;
        OnShoot?.Invoke();
    }
}
