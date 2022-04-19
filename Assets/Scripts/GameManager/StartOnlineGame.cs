using Photon.Pun;

public class StartOnlineGame : BaseStartGame
{
    [PunRPC]
    private void OnPlayersReadyStartTheGame()
    {
        _gameManager.OnGameStarted?.Invoke();
        GameManager.IsGameStarted = true;
        print("Online game has started");
    }

    protected override void StartGame()
    {
        if (GameManager.MasterPlayerReady && GameManager.SecondPlayerReady)
        {
            photonView.RPC("OnPlayersReadyStartTheGame", RpcTarget.AllViaServer);
        }
    }

    protected override bool CanStartGame()
    {
        return photonView.IsMine && !MyPhotonNetwork.IsOfflineMode && !GameManager.IsGameStarted;
    }     
}
