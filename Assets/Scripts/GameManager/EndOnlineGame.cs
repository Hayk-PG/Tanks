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
    
    protected override void OnGameEnded(string successedPlayerName, string defeatedPlayerName)
    {
        photonView.RPC("RPC", RpcTarget.AllViaServer, successedPlayerName, defeatedPlayerName);
    }

    [PunRPC]
    private void RPC(string successedPlayerName, string defeatedPlayerName)
    {
        print(successedPlayerName + " won/" + defeatedPlayerName + " lost");
        _gameManager.OnGameEnded?.Invoke();
        OnEndGameTab?.Invoke(successedPlayerName, defeatedPlayerName);
        UnsubscribeFromPluginService();       
    }
}
