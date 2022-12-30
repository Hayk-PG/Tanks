using UnityEngine;

public class AAProjectileLauncher : MonoBehaviour
{
    [SerializeField] private BulletController[] _missiles;

    private TurnController _turnController;
    private PhotonNetworkAALauncher _photonNetworkAALauncher;
    private IScore _ownerScore;
    private BulletController _enemyBullet;

    [SerializeField] private float _force;
    private int index = 0;
    private bool _launched;

    public BulletController[] Missiles => _missiles;
    public Vector3 ID { get; private set; }



    private void Awake()
    {
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
        _ownerScore = Get<IScore>.From(ownerTankController.gameObject);
    }

    private void LaunchMissile()
    {
        if (IsTargetDetected())
        {
            if (!_launched && index < _missiles.Length)
            {
                Conditions<bool>.Compare(MyPhotonNetwork.IsOfflineMode, InitMissile, () => InitMissile(_photonNetworkAALauncher));
                _launched = true;
            }
        }
    }

    public void InitMissile()
    {
        _missiles[index].gameObject.SetActive(true);
        _missiles[index].OwnerScore = _ownerScore;
        _missiles[index].RigidBody.velocity = _missiles[index].transform.TransformDirection(Vector3.forward) * _force;
        index++;
    }

    private void InitMissile(PhotonNetworkAALauncher photonNetworkAALauncher)
    {
        photonNetworkAALauncher.InitMissile(ID);
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
