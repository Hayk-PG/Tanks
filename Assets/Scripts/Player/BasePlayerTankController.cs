using Photon.Pun;
using UnityEngine;

public abstract class BasePlayerTankController<T> : MonoBehaviourPun
{
    public T OwnTank { get; set; }
    public abstract void GetControl(T tank);
}
