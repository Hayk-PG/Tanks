using Photon.Pun;
using UnityEngine;

public abstract class BasePlayerTankController<T> : MonoBehaviourPun
{
    public TankController OwnTank { get; set; }
    protected T _playerController;

    internal TankMovement _tankMovement;
    internal Rigidbody _tankRigidbody;


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
    }

    protected abstract void GetTankControl();
}
