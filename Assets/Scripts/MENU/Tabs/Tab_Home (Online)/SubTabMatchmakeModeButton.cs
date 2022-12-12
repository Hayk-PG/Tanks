public class SubTabMatchmakeModeButton : OnlineGameModes<SubTabsButton>
{
    private void OnEnable()
    {
        if (_broadcaster == null)
            return;

        _broadcaster.onSelect += Select;
    }

    private void OnDisable()
    {
        if (_broadcaster == null)
            return;

        _broadcaster.onSelect -= Select;
    }

    private void Select() => MyPhoton.JoinLobby(Names.LobbyDefault, Photon.Realtime.LobbyType.Default);
}
