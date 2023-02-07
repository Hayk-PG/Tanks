public class EndOfflineGame : BaseEndGame
{
    protected override void OnEnable()
    {
        if (MyPhotonNetwork.IsOfflineMode)
        {
            base.OnEnable();
        }
    }

    protected override void OnGameEnded(string successedPlayerName, string defeatedPlayerName)
    {
        base.OnGameEnded(successedPlayerName, defeatedPlayerName);       
    }
}
