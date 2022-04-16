
public class StartOfflineGame : BaseStartGame
{
    protected override bool CanStartGame()
    {
        return MyPhotonNetwork.IsOfflineMode && GameManager.MasterPlayerReady && !GameManager.IsGameStarted;
    }

    protected override void StartGame()
    {
        _gameManager.OnGameStarted?.Invoke();
        GameManager.IsGameStarted = true;
    }
}
