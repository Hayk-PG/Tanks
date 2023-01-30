using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

//ADDRESSABLE
public class AALauncher : MonoBehaviour
{
    [SerializeField] private AssetReference _assetReferenceMesh;
    [SerializeField] private AssetReference _assetReferenceGunMesh;
    [SerializeField] private AssetReference _assetReferenceAaGun;
    [SerializeField] private AssetReference _assetReferenceSpawnParticles;
    [SerializeField] private AssetReference _assetReferenceDespawnEffect;

    [Space]
    [SerializeField] private Transform _meshTransform;
    [SerializeField] private Transform _gunMeshTrasnform;
    [SerializeField] private Transform[] _points;    

    private GameObject _meshObj;
    private GameObject _gunMeshObj;
   
    private Animator _animator;
    private TurnController _turnController;
    private PhotonNetworkAALauncher _photonNetworkAALauncher;
    private IScore _ownerScore;
    private BulletController _enemyProjectile;
    private AAGun _aAGun;

    [Space]
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
       
        InstantiateSpawnParticlesAsync();
        InstantiateMeshesAsync();

        ID = transform.position;
        _ownerScore = Get<IScore>.From(ownerTankController.gameObject);

        print(_ownerScore);
    }

    private void InstantiateMeshesAsync()
    {
        _assetReferenceMesh.InstantiateAsync(_meshTransform).Completed += (asset) => { _meshObj = asset.Result; };
        _assetReferenceGunMesh.InstantiateAsync(_gunMeshTrasnform).Completed += (asset) => { _gunMeshObj = asset.Result; };
    }

    private void InstantiateSpawnParticlesAsync()
    {
        _assetReferenceSpawnParticles.InstantiateAsync(transform.position, Quaternion.Euler(-90, 0, 0), null);
    }

    private void LaunchMissile()
    {
        if (IsTargetDetected() && !_launched)
        {
            if (_index < _points.Length)
            {
                Conditions<bool>.Compare(MyPhotonNetwork.IsOfflineMode, InstantiateGun, () => InstantiateGun(_photonNetworkAALauncher));
                _launched = true;
            }
            else
            {
                if (!_isDeactivated)
                {
                    Conditions<bool>.Compare(MyPhotonNetwork.IsOfflineMode, Deactivate, () => Deactivate(_photonNetworkAALauncher));
                    _isDeactivated = true;
                }
            }
        }
    }

    public void InstantiateGun()
    {
        DestroyAAGun();
        _assetReferenceAaGun.InstantiateAsync(_points[_index]).Completed += InitProjectile;
    }

    private void InstantiateGun(PhotonNetworkAALauncher photonNetworkAALauncher) => photonNetworkAALauncher.InstantiateGun(ID);

    private void InitProjectile(AsyncOperationHandle<GameObject> result)
    {
        _aAGun = Get<AAGun>.From(result.Result);
        _aAGun.Projectile.transform.SetParent(null);
        _aAGun.Projectile.gameObject.SetActive(true);
        _aAGun.Projectile.OwnerScore = _ownerScore;
        _aAGun.Projectile.RigidBody.velocity = _aAGun.transform.TransformDirection(Vector3.forward) *_force;
        _animator.Play(_animationLaunch, 0, 0);
        _index++;
    }

    public void Deactivate() => Addressables.InstantiateAsync(_assetReferenceDespawnEffect).Completed += OnDeactivate;

    private void Deactivate(PhotonNetworkAALauncher photonNetworkAALauncher) => photonNetworkAALauncher.Deactivate(ID);

    private void OnDeactivate(AsyncOperationHandle<GameObject> result)
    {
        result.Result.transform.position = transform.position + (Vector3.up / 2);
        DestroyAAGun();
        DestroyAALauncher();
    }

    private void DestroyAAGun()
    {
        if (_aAGun != null)
        {
            _assetReferenceAaGun.ReleaseInstance(_aAGun.gameObject);
            Destroy(_aAGun.gameObject);
        }
    }

    private void DestroyAALauncher()
    {
        Addressables.ReleaseInstance(gameObject);
        Addressables.ReleaseInstance(_meshObj);
        Addressables.ReleaseInstance(_gunMeshObj);
        Destroy(gameObject);
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
