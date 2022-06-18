using Photon.Pun;
using UnityEngine;

public abstract class BasePlayerTankController<T> : MonoBehaviourPun
{
    [SerializeField] protected TankController _tankController;

    public TankController OwnTank
    {
        get => _tankController;
        protected set => _tankController = value;
    }
    protected T _playerController;

    internal TankMovement _tankMovement;   
    internal Rigidbody _tankRigidbody;
    internal ShootController _shootController;
    internal PlayerTurn _playerTurn;
    internal HealthController _healthController;
    internal HealthBar _healthBar;
    internal PlayerAmmoType _playerAmmoType;
    internal PlayerRequestAirSupport _playerRequestAirSupport;
    internal PlayerDeployProps _playerDeployProps;


    protected virtual void Awake()
    {
        _playerController = Get<T>.From(gameObject);
    }

    public virtual void CacheTank(TankController tank)
    {
        OwnTank = tank;
        GetTankControl();
        _tankMovement = OwnTank?.GetComponent<TankMovement>();       
        _tankRigidbody = OwnTank?.GetComponent<Rigidbody>();
        _shootController = OwnTank?.GetComponent<ShootController>();
        _playerTurn = OwnTank?.GetComponent<PlayerTurn>();
        _healthController = OwnTank?.GetComponent<HealthController>();
        _healthBar = OwnTank?.GetComponentInChildren<HealthBar>();
        _playerAmmoType = OwnTank?.GetComponent<PlayerAmmoType>();
        _playerRequestAirSupport = OwnTank?.GetComponent<PlayerRequestAirSupport>();
        _playerDeployProps = OwnTank?.GetComponent<PlayerDeployProps>();
    }

    protected abstract void GetTankControl();
}
