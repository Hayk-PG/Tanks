using System;

public class Tab_StartGame : Tab_Base<MyPhotonCallbacks>
{
    public event Action onPlayOffline;
    public event Action onPlayOnline; 


    public void OnClickVsAiButton()
    {
        MyPhotonNetwork.OfflineMode(true);
        onPlayOffline?.Invoke();
    }

    public void OnClickVsPlayerButton()
    {
        MyPhotonNetwork.OfflineMode(false);
        onPlayOnline?.Invoke();
    }

    public override void OpenTab()
    {
        MyPhoton.LeaveRoom();
        MyPhoton.LeaveLobby();
        MyPhoton.Disconnect();

        base.OpenTab();
    }
}
