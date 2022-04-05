using UnityEngine;

public class Tab_Lobby : Tab_Base<MyPhotonCallbacks>
{
    private void OnEnable()
    {
        _object.OnPhotonJoinedLobby += OnPhotonJoinedLobby;
    }

    private void OnDisable()
    {
        _object.OnPhotonJoinedLobby -= OnPhotonJoinedLobby;
    }

    private void OnPhotonJoinedLobby()
    {
        MenuTabs.Activity(MenuTabs.Tab_Lobby.CanvasGroup);
    }
}
