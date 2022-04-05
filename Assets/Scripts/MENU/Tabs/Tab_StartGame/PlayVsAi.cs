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
        MySceneManager.Instance.LoadScene(MySceneManager.SceneName.Game);
    }
}
