using Photon.Pun;
using UnityEngine;

public abstract class BasePlayerTankController<T> : MonoBehaviourPun
{
    [SerializeField] 
    protected TankController _tankController;

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
    internal PlayerHUD _playerHud;
    internal HealthController _healthController;
    internal ScoreController _scoreController;
    internal IScore _iScore;
    internal HealthBar _healthBar;
    internal PlayerAmmoType _playerAmmoType;
    internal PlayerShields _playerShields;


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
        _playerHud = OwnTank?.GetComponentInChildren<PlayerHUD>();
        _healthController = OwnTank?.GetComponent<HealthController>();
        _scoreController = OwnTank?.GetComponent<ScoreController>();
        _iScore = OwnTank?.GetComponent<IScore>();
        _healthBar = OwnTank?.GetComponentInChildren<HealthBar>();
        _playerAmmoType = OwnTank?.GetComponent<PlayerAmmoType>();
        _playerShields = OwnTank?.GetComponent<PlayerShields>();
    }

    protected abstract void GetTankControl();
}
