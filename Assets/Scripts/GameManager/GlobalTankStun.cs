using Photon.Pun;

public class GlobalTankStun : MonoBehaviourPun
{
    public void OnStunned(TurnState turnState, float duration)
    {
        if(MyPhotonNetwork.AmPhotonViewOwner(photonView))
            photonView.RPC("StunnedRPC", RpcTarget.AllViaServer, turnState, duration);
    }

    [PunRPC]
    private void StunnedRPC(TurnState turnState, float duration)
    {
        Stun stun = Get<Stun>.FromChild(GlobalFunctions.ObjectsOfType<PlayerTurn>.Find(tank => tank.MyTurn == turnState).gameObject);
        stun.OnStunned(duration);
    }

    public void OnDisableStunningEffect(TurnState turnState)
    {
        if(MyPhotonNetwork.AmPhotonViewOwner(photonView))
            photonView.RPC("DisableStunningEffectRPC", RpcTarget.AllViaServer, turnState);
    }

    [PunRPC]
    private void DisableStunningEffectRPC(TurnState turnState)
    {
        Stun stun = Get<Stun>.FromChild(GlobalFunctions.ObjectsOfType<PlayerTurn>.Find(tank => tank.MyTurn == turnState).gameObject);
        stun.OnDisableStunningEffect();
    }
}
