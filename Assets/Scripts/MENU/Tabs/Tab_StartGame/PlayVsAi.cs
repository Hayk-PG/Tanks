using System;

public class PlayVsAi : BasePlayVs
{
    public Action OnClickedPlayVsAIButton { get; set; }

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
        OnClickedPlayVsAIButton?.Invoke();
    }
}
