using System;

public class Tab_SelectedTanks : Tab_Base<MyPhotonCallbacks>
{
    private PlayVsAi _playVsAi;
    public Action OnPhotonOnlineTankSelected { get; set; }
    public Action OnOfflineTankSelected { get; set; }


    protected override void Awake()
    {
        base.Awake();
        _playVsAi = FindObjectOfType<PlayVsAi>();
    }

    private void OnEnable()
    {
        _playVsAi.OnClickedPlayVsAIButton += base.OpenTab;
        _object._OnJoinedLobby += base.OpenTab;
    }

    private void OnDisable()
    {
        _playVsAi.OnClickedPlayVsAIButton -= base.OpenTab;
        _object._OnJoinedLobby -= base.OpenTab;
    }

    public void OnClickSelectButton()
    {
        if (MyPhotonNetwork.IsOfflineMode)
            OnOfflineTankSelected?.Invoke();
        else
            OnPhotonOnlineTankSelected?.Invoke();
    }
}
