using System.Collections;
using UnityEngine;

public class AIShootController : BaseShootController
{
    [SerializeField] [Space]
    private WeaponProperties[] _bulletsPrefab;

    [SerializeField] [Space]
    private int _activeBulletIndex;

    private struct BulletsList
    {
        internal WeaponProperties _weaponProperties;
        internal int _count;
        internal bool _isUnlockingTimerRunning;
    }
    private BulletsList[] _cachedBulletsList;

    private Rigidbody _rigidBody;

    private AITankMovement _aiTankMovement;

    private AIEnemyDataGetter _aiEnemyDataGetter;

    private IScore _iScore;
    private IDamage _iDamage;

    private Vector3 _target;
    
    private Quaternion _lookRot;
    private Quaternion _rot;
    private Quaternion _desiredRotation;
     
    private float _defaultTrajectoryTime = 1;
    private float _currentTrajectoryTime = 1;
    private float _currentOffsetX = 0;
    private float _targetFixingValue = 0;

    private bool CanRotateCanon
    {
        get
        {
            return Converter.AngleConverter(_canon._currentEulerAngleX) < _canon._maxEulerAngleX &&
                   Converter.AngleConverter(_canon._currentEulerAngleX) > _canon._minEulerAngleX;
        }
    }

    public event System.Action<GameObject> onAiShoot;





    protected override void Awake()
    {
        base.Awake();

        _rigidBody = GetComponent<Rigidbody>();

        _aiTankMovement = GetComponent<AITankMovement>();

        _aiEnemyDataGetter = Get<AIEnemyDataGetter>.From(gameObject);

        _iScore = Get<IScore>.From(gameObject);

        _iDamage = Get<IDamage>.From(gameObject);

        InitializeBulletsList(_bulletsPrefab.Length);

        _activeBulletIndex = 0;
    }

    private void Start()
    {
        AIShootBehaviour(Data.Manager.SingleGameDifficultyLevel);
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        _aiTankMovement.Shoot += ShootBullet;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        _aiTankMovement.Shoot -= ShootBullet;

        _aiCanonRaycast.onRayCast -= OnAICanonRaycast;
    }

    private void Update()
    {
        if(!_isStunned)
            RotateCanon();
    }

    private void InitializeBulletsList(int length)
    {
        _cachedBulletsList = new BulletsList[length];

        for (int i = 0; i < _cachedBulletsList.Length; i++)
        {
            _cachedBulletsList[i]._weaponProperties = _bulletsPrefab[i];
            _cachedBulletsList[i]._count = _bulletsPrefab[i]._value;
            _cachedBulletsList[i]._isUnlockingTimerRunning = false;
        }
    }

    private bool HaveEnoughBulletsCount(int index)
    {
        return _cachedBulletsList[index]._count > 0;
    }

    private bool HaveRequiredScoreAmmount(int index)
    {
        return _scoreController.Score >= _bulletsPrefab[_activeBulletIndex]._requiredScoreAmmount;
    }

    private bool IsCurrentWeaponUnclockingTimerRunning(int index)
    {
        return _cachedBulletsList[index]._isUnlockingTimerRunning;
    }

    private void UpdateBulletsCount(int index, int number)
    {
        _cachedBulletsList[index]._count += number;
    }

    private void UsedActiveWeapon()
    {
        UpdateBulletsCount(_activeBulletIndex, -1);

        if (_activeBulletIndex > 0 && !IsCurrentWeaponUnclockingTimerRunning(_activeBulletIndex))
            StartCoroutine(RunWeaponTimer(_activeBulletIndex));
    }

    private void ActivateRandomWeapon()
    {
        int randomIndex = Random.Range(0, _cachedBulletsList.Length);

        if (HaveEnoughBulletsCount(randomIndex))
        {
            _activeBulletIndex = randomIndex;
        }

        if(!HaveEnoughBulletsCount(randomIndex) && !IsCurrentWeaponUnclockingTimerRunning(randomIndex) && HaveRequiredScoreAmmount(randomIndex))
        {         
            UpdateBulletsCount(randomIndex, _bulletsPrefab[randomIndex]._value);

            _scoreController.Score -= _bulletsPrefab[randomIndex]._requiredScoreAmmount;

            _activeBulletIndex = randomIndex;
        }

        if(!HaveEnoughBulletsCount(randomIndex) && IsCurrentWeaponUnclockingTimerRunning(randomIndex))
        {
            _activeBulletIndex = 0;
        }
    }

    private IEnumerator RunWeaponTimer(int weaponIndex)
    {
        _cachedBulletsList[weaponIndex]._isUnlockingTimerRunning = true;

        float unlockTime = (_bulletsPrefab[weaponIndex]._minutes * 60) + _bulletsPrefab[weaponIndex]._seconds;

        while (unlockTime > 0)
        {
            unlockTime--;

            yield return new WaitForSeconds(1);
        }

        yield return null;

        if (unlockTime == 0)
        {
            _cachedBulletsList[weaponIndex]._isUnlockingTimerRunning = false;
        }
    }

    private void SubscribeToAICanonRaycast()
    {
        if (_aiCanonRaycast != null)
            _aiCanonRaycast.onRayCast += OnAICanonRaycast; 
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

                break;
        }
    }

    private IEnumerator OnHigherThanEasyDifficultyLevel()
    {
        while (true)
        {
            yield return new WaitForSeconds(2);
        }
    }

    private void OnAICanonRaycast(bool isRaycastHit, float hitPointDistance)
    {
        if (CanRotateCanon)
        {
            //return;
        }

        if (isRaycastHit)
        {
            if (hitPointDistance > _aiEnemyDataGetter.Distance || hitPointDistance <= _aiEnemyDataGetter.Distance && _aiEnemyDataGetter.Distance > 8)
                _currentOffsetX = Mathf.Lerp(0, _aiEnemyDataGetter.Distance, _currentTrajectoryTime) / 10;

            if (hitPointDistance <= _aiEnemyDataGetter.Distance && _aiEnemyDataGetter.Distance < 8)
                _currentOffsetX = 0;

            _currentTrajectoryTime = _currentTrajectoryTime < 2 ? _currentTrajectoryTime += 2 * Time.fixedDeltaTime : 2;
        }
        else
        {
            if (_aiEnemyDataGetter.Distance > 8)
                _currentOffsetX = Mathf.Lerp(0, _aiEnemyDataGetter.Distance, _currentTrajectoryTime) / 10;
            else
                _currentOffsetX = 0;
        }
    }

    public void RotateCanon()
    {
        _target = _trajectory.PredictedTrajectory(new Vector3((_aiEnemyDataGetter.Enemy.position.x - _currentOffsetX) + _targetFixingValue, _aiEnemyDataGetter.Enemy.position.y, _aiEnemyDataGetter.Enemy.position.z), transform.position, _currentTrajectoryTime);

        _lookRot = Quaternion.LookRotation(Vector3.forward, _target);

        _rot = _lookRot * Quaternion.Euler(_canon._rotationStabilizer.x, _canon._rotationStabilizer.y, _canon._rotationStabilizer.z);

        _desiredRotation = Quaternion.Slerp(_desiredRotation, _rot, _canon._rotationSpeed);

        _canon._rotationSpeed = 2 * Time.deltaTime;

        _canon._currentEulerAngleX = _desiredRotation.eulerAngles.x;

        _canonPivotPoint.rotation = _desiredRotation;
    }

    private void ShootBullet()
    {
        if(!_isStunned)
            StartCoroutine(ShootBulletCoroutine());
    }

    private IEnumerator ShootBulletCoroutine()
    {
        yield return new WaitForSeconds(2);

        if (HaveEnoughBulletsCount(_activeBulletIndex))
        {
            BaseBulletController baseBulletController = Instantiate(_cachedBulletsList[_activeBulletIndex]._weaponProperties._prefab, _shootPoint.position, _canonPivotPoint.rotation);

            baseBulletController.OwnerScore = _iScore;
            baseBulletController.IDamageAi = _iDamage;
            baseBulletController.IsOwnerAI = true;
            baseBulletController.RigidBody.velocity = _target;

            _rigidBody.AddForce(transform.forward * _target.magnitude * _shoot._rigidbodyForceMultiplier, ForceMode.Impulse);

            //GameSceneObjectsReferences.MainCameraController.CameraOffset(_playerTurn, baseBulletController.RigidBody, 5, null);

            _currentTrajectoryTime = _defaultTrajectoryTime;
            
            OnDash(_aiTankMovement.Direction);

            UsedActiveWeapon();

            ActivateRandomWeapon();

            OnShoot?.Invoke();

            onAiShoot?.Invoke(baseBulletController.gameObject);
        }
    }

    public void ControlTargetFixingValue(float targetFixingValue)
    {
        _targetFixingValue = targetFixingValue;
    }
}
