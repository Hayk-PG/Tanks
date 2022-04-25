using UnityEngine;
using Photon.Pun;

public class EndOnlineGame : BaseEndGame
{
    protected override void OnEnable()
    {
        if (!MyPhotonNetwork.IsOfflineMode)
        {
            base.OnEnable();
        }
    }

    protected override void OnPluginService()
    {
        if (MyPhotonNetwork.AmPhotonViewOwner(photonView))
        {
            base.OnPluginService();
        }
    }
    
    protected override void OnGameEnded(string playHasWon)
    {
        photonView.RPC("RPC", RpcTarget.AllViaServer, playHasWon);
    }

    [PunRPC]
    private void RPC(string playHasWon)
    {
        print(playHasWon);
        UnsubscribeFromPluginService();
    }
}
