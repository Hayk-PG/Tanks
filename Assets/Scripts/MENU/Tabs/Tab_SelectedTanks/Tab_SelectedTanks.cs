using System;

public class Tab_SelectedTanks : Tab_Base<MyPhotonCallbacks>
{
    private PlayVsAi _playVsAi;

    public Action OnSingleGameTankSelected { get; set; }
    public Action OnOnlineGameTankChanged { get; set; }
    


    protected override void Awake()
    {
        base.Awake();
        _playVsAi = FindObjectOfType<PlayVsAi>();
    }

    private void OnEnable()
    {
        _playVsAi.OnClickedPlayVsAIButton += base.OpenTab;
        MyPhoton.OnNickNameSet += base.OpenTab;
    }

    private void OnDisable()
    {
        _playVsAi.OnClickedPlayVsAIButton -= base.OpenTab;
        MyPhoton.OnNickNameSet -= base.OpenTab;
    }

    public void OnClickSelectButton()
    {
        if (MyPhotonNetwork.IsOfflineMode)
        {
            OnSingleGameTankSelected?.Invoke();
        }
        else
        {
            if (!MyPhotonNetwork.IsInRoom)
                MyPhoton.JoinLobby();
            if (MyPhotonNetwork.IsInRoom)
                OnOnlineGameTankChanged?.Invoke(); ;
        }
    }
}
