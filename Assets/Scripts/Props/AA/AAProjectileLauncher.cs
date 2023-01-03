using UnityEngine;

public class AAProjectileLauncher : MonoBehaviour
{
    private Animator _animator;
    private TurnController _turnController;
    private PhotonNetworkAALauncher _photonNetworkAALauncher;
    private IScore _ownerScore;
    private BulletController _enemyBullet;
    private AAProjectileArsenal _arsenal;

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
                    Conditions<bool>.Compare(MyPhotonNetwork.IsOfflineMode, InitMissile, () => InitMissile(_photonNetworkAALauncher));
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

    public void InitMissile()
    {
        LoadAssetAsync();
    }

    private void InitMissile(PhotonNetworkAALauncher photonNetworkAALauncher)
    {
        photonNetworkAALauncher.InitMissile(ID);
    }

    private void LoadAssetAsync()
    {
        MyAddressable.LoadAssetAsync((string)AddressablesPath.AAProjectileArsenal[0, 0], (int)AddressablesPath.AAProjectileArsenal[0, 1], true,
            delegate (GameObject gameObject)
            {
                Shoot(gameObject);
            },
            null);
    }

    private void Shoot(GameObject gameObject)
    {
        if (_arsenal != null)
            Destroy(_arsenal);

        _arsenal = Get<AAProjectileArsenal>.From(Instantiate(gameObject, _points[_index]));
        _arsenal.Missile.gameObject.SetActive(true);
        _arsenal.Missile.OwnerScore = _ownerScore;
        _arsenal.Missile.RigidBody.velocity = _arsenal.transform.TransformDirection(Vector3.forward) * _force;
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
                gameObject.SetActive(false);
            },
            null);
    }

    private void Deactivate(PhotonNetworkAALauncher photonNetworkAALauncher)
    {
        photonNetworkAALauncher.Deactivate(ID);
    }

    private void OnTurnChanged(TurnState turnState)
    {
        if(turnState == TurnState.Other)
        {
            _enemyBullet = GlobalFunctions.ObjectsOfType<BulletController>.Find(bullet => bullet.OwnerScore != _ownerScore);
            _launched = false;
        }
    }

    private bool IsTargetDetected()
    {
        return _enemyBullet != null && Vector3.Distance(_enemyBullet.transform.position, transform.position) <= 10 ? true : false;
    }
}
