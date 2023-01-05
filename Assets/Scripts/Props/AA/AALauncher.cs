using UnityEngine;

public class AALauncher : MonoBehaviour
{
    private Animator _animator;
    private TurnController _turnController;
    private PhotonNetworkAALauncher _photonNetworkAALauncher;
    private IScore _ownerScore;
    private BulletController _enemyProjectile;
    private AAGun _aAGun;

    [SerializeField] private Transform[] _points;
    [SerializeField] private ParticleSystem _gameobjectSpawnEffect;
    [SerializeField] private float _force;
    private int _index = 0;
    private bool _launched;
    private bool _isDeactivated;
    private string _animationLaunch = "Launch";

    public Vector3 ID { get; private set; }



    private void Awake()
    {
        _animator = Get<Animator>.From(gameObject);
        _turnController = FindObjectOfType<TurnController>();
        _photonNetworkAALauncher = FindObjectOfType<PhotonNetworkAALauncher>();
    }

    private void OnEnable() => _turnController.OnTurnChanged += OnTurnChanged;

    private void OnDisable() => _turnController.OnTurnChanged -= OnTurnChanged;

    private void FixedUpdate() => LaunchMissile();

    public void Init(TankController ownerTankController)
    {
        if (ownerTankController == null)
            return;

        ID = transform.position;
        _index = 0;
        _isDeactivated = false;
        _ownerScore = Get<IScore>.From(ownerTankController.gameObject);
        _gameobjectSpawnEffect.Play(true);
        print(_ownerScore);
    }

    private void LaunchMissile()
    {
        if (IsTargetDetected())
        {
            if (!_launched)
            {
                if (_index < _points.Length)
                {
                    Conditions<bool>.Compare(MyPhotonNetwork.IsOfflineMode, InstantiateGun, () => InstantiateGun(_photonNetworkAALauncher));
                    _launched = true;
                }
                else
                {
                    if(!_isDeactivated)
                    {
                        Conditions<bool>.Compare(MyPhotonNetwork.IsOfflineMode, Deactivate, () => Deactivate(_photonNetworkAALauncher));
                        _isDeactivated = true;
                    }
                }
            }
        }
    }

    public void InstantiateGun()
    {
        DestroyAAGun();
        MyAddressable.InstantiateAsync((string)AddressablesPath.AAGun[0, 0], _points[_index], InitMissile);
    }

    private void InstantiateGun(PhotonNetworkAALauncher photonNetworkAALauncher)
    {
        photonNetworkAALauncher.InstantiateGun(ID);
    }

    private void InitMissile(GameObject gun)
    {
        _aAGun = Get<AAGun>.From(gun);
        _aAGun.Projectile.transform.SetParent(null);
        _aAGun.Projectile.gameObject.SetActive(true);
        _aAGun.Projectile.OwnerScore = _ownerScore;
        _aAGun.Projectile.RigidBody.velocity = _aAGun.transform.TransformDirection(Vector3.forward) *_force;
        _animator.Play(_animationLaunch, 0, 0);
        _index++;
    }

    public void Deactivate()
    {
        MyAddressable.LoadAssetAsync((string)AddressablesPath.AAProjectileArsenalDespawnEffect[0, 0], (int)AddressablesPath.AAProjectileArsenalDespawnEffect[0, 1], true,
            (obj) =>
            {
                GameObject particle = Instantiate(obj);
                particle.transform.position = transform.position + (Vector3.up / 2);
                DestroyAAGun();
                gameObject.SetActive(false);
            },
            null);
    }

    private void Deactivate(PhotonNetworkAALauncher photonNetworkAALauncher)
    {
        photonNetworkAALauncher.Deactivate(ID);
    }

    private void DestroyAAGun()
    {
        if (_aAGun != null)
        {
            MyAddressable.ReleaseInstance(_aAGun.gameObject);
            Destroy(_aAGun.gameObject);
        }
    }

    private void OnTurnChanged(TurnState turnState)
    {
        if(turnState == TurnState.Other)
        {
            _enemyProjectile = GlobalFunctions.ObjectsOfType<BulletController>.Find(bullet => bullet.OwnerScore != _ownerScore);
            _launched = false;
        }
    }

    private bool IsTargetDetected()
    {
        return _enemyProjectile != null && Vector3.Distance(_enemyProjectile.transform.position, transform.position) <= 10 ? true : false;
    }
}
