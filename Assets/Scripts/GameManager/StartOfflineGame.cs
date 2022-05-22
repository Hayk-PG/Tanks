
public class StartOfflineGame : BaseStartGame
{
    protected override bool CanStartGame()
    {
        return MyPhotonNetwork.IsOfflineMode && _gameManager.MasterPlayerReady && !_gameManager.IsGameStarted;
    }

    protected override void StartGame()
    {
        _gameManager.OnGameStarted?.Invoke();
        _gameManager.IsGameStarted = true;
    }
}
