using Photon.Pun;

public class StartOnlineGame : BaseStartGame
{
    [PunRPC]
    private void OnPlayersReadyStartTheGame()
    {
        if (GameManager.MasterPlayerReady && GameManager.SecondPlayerReady)
        {
            _gameManager.OnGameStarted?.Invoke();
            GameManager.IsGameStarted = true;
        }
    }

    protected override void StartGame()
    {
        photonView.RPC("OnPlayersReadyStartTheGame", RpcTarget.AllViaServer);
    }

    protected override bool CanStartGame()
    {
        return photonView.IsMine && !MyPhotonNetwork.IsOfflineMode && !GameManager.IsGameStarted;
    }     
}
