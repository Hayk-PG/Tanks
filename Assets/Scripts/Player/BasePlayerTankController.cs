﻿using Photon.Pun;
using UnityEngine;

public abstract class BasePlayerTankController<T> : MonoBehaviourPun
{
    public TankController OwnTank { get; set; }
    protected T _playerController;

    internal TankMovement _tankMovement;   
    internal Rigidbody _tankRigidbody;
    internal ShootController _shootController;
    internal PlayerTurn _playerTurn;


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
    }

    protected abstract void GetTankControl();
}
