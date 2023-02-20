﻿using Photon.Pun;
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
    internal HealthBar _healthBar;
    internal PlayerAmmoType _playerAmmoType;
    internal PlayerRequestAirSupport _playerRequestAirSupport;
    internal PlayerDeployProps _playerDeployProps;
    internal PlayerShields _playerShields;
    internal PlayerDeployMetalCube _playerDeployMetalCube;
    internal PlayerChangeTileToMetalGround _playerChangeTileToMetalGround;
    internal PlayerTankSkipTurn _playerTankSkipTurn;


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
        _healthBar = OwnTank?.GetComponentInChildren<HealthBar>();
        _playerAmmoType = OwnTank?.GetComponent<PlayerAmmoType>();
        _playerRequestAirSupport = OwnTank?.GetComponent<PlayerRequestAirSupport>();
        _playerDeployProps = OwnTank?.GetComponent<PlayerDeployProps>();
        _playerShields = OwnTank?.GetComponent<PlayerShields>();
        _playerDeployMetalCube = OwnTank?.GetComponent<PlayerDeployMetalCube>();
        _playerChangeTileToMetalGround = OwnTank?.GetComponent<PlayerChangeTileToMetalGround>();
        _playerTankSkipTurn = OwnTank?.GetComponent<PlayerTankSkipTurn>();
    }

    protected abstract void GetTankControl();
}
