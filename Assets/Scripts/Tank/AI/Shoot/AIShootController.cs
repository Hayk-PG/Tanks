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

    private delegate float TempValue();
    private TempValue TrajectoryTimeOffset { get; set; }
    private TempValue TrajectoryTimeReset
    {
        get => delegate { return Mathf.Lerp(TrajectoryTime, 1, 1 * Time.deltaTime); };
    }
    private float TrajectoryTime { get; set; } = 1;
    private float TargetOffsetX { get; set; } = 0;
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
                TargetOffsetX = 0.5f;
                TrajectoryTimeOffset = delegate { return Mathf.Lerp(TrajectoryTime, 1.5f, 1 * Time.deltaTime); };
                break;

            case SingleGameDifficultyLevel.Hard:
                SubscribeToAICanonRaycast();
                TargetOffsetX = 1;
                TrajectoryTimeOffset = delegate { return Mathf.Lerp(TrajectoryTime, 2, 1 * Time.deltaTime); };
                break;
        }
    }

    private void OnAICanonRaycast(float distance, bool isRaycastHit)
    {
        if (CanRotateCanon)
        {
            if (distance > 3 && TrajectoryTime < 2 || distance <= 3 && distance > 0 && TrajectoryTime < 1.5f && isRaycastHit)
                TrajectoryTime = TrajectoryTimeOffset();
            if (!isRaycastHit && distance < 3)
                TrajectoryTime = TrajectoryTimeReset();
        }
    }

    public void RotateCanon()
    {
        _target = _trajectory.PredictedTrajectory(new Vector3(_player.position.x - TargetOffsetX, _player.position.y, _player.position.z), transform.position, TrajectoryTime);
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
        TrajectoryTime = 1;
        OnShoot?.Invoke();
    }
}
