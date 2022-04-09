public class PlayVsAi : BasePlayVs
{  
    private void OnEnable()
    {
        _tabStartGame.OnStartPlayVsAi += OnStartPlayVsAi;
    }

    private void OnDisable()
    {
        _tabStartGame.OnStartPlayVsAi -= OnStartPlayVsAi;
    }

    private void OnStartPlayVsAi()
    {
        MyPhotonNetwork.OfflineMode(true);
        MyScene.Manager.LoadScene(MyScene.SceneName.Game);
    }
}
