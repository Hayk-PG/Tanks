using Photon.Pun;
using UnityEngine;


public class GlobalTankStun : MonoBehaviourPun
{
    private GameObject _stunnedTank;


    public void OnStunned(TurnState turnState, float duration)
    {
        if(MyPhotonNetwork.AmPhotonViewOwner(photonView))
            photonView.RPC("StunnedRPC", RpcTarget.AllViaServer, turnState, duration);
    }

    [PunRPC]
    private void StunnedRPC(TurnState turnState, float duration)
    {
        _stunnedTank = (GlobalFunctions.ObjectsOfType<PlayerTurn>.Find(tank => tank.MyTurn == turnState)?.gameObject);

        if(_stunnedTank != null)
        {
            Stun stun = Get<Stun>.FromChild(_stunnedTank);
            stun?.OnStunned(duration);
        }
    }

    public void OnDisableStunningEffect(TurnState turnState)
    {
        if(MyPhotonNetwork.AmPhotonViewOwner(photonView))
            photonView.RPC("DisableStunningEffectRPC", RpcTarget.AllViaServer, turnState);
    }

    [PunRPC]
    private void DisableStunningEffectRPC(TurnState turnState)
    {
        _stunnedTank = (GlobalFunctions.ObjectsOfType<PlayerTurn>.Find(tank => tank.MyTurn == turnState)?.gameObject);

        if (_stunnedTank != null)
        {
            Stun stun = Get<Stun>.FromChild(_stunnedTank);
            stun?.OnDisableStunningEffect();
        }
    }
}
