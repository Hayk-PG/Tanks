using Photon.Pun;

public class StartOnlineGame : BaseStartGame
{
    protected override void StartGame()
    {
        if (_gameManager.MasterPlayerReady && _gameManager.SecondPlayerReady)
        {
            photonView.RPC("OnPlayersReadyStartTheGame", RpcTarget.AllViaServer);

            _gameManager.IsGameStarted = true;
        }
    }

    [PunRPC]
    private void OnPlayersReadyStartTheGame()
    {
        _gameManager.OnGameStarted?.Invoke();

        _gameManager.IsGameStarted = true;
    }

    protected override bool CanStartGame()
    {
        return MyPhotonNetwork.AmPhotonViewOwner(photonView) && !MyPhotonNetwork.IsOfflineMode && !_gameManager.IsGameStarted;
    }     
}
