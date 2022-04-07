using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;

public partial class MyPhotonCallbacks : MonoBehaviourPunCallbacks
{
    public Action OnLobbyLeft { get; set; }
    public Action OnRoomLeft { get; set; }
    public Action<Player> OnRoomPlayerLeft { get; set; }


    public override void OnLeftLobby()
    {
        OnLobbyLeft?.Invoke();
    }

    public override void OnLeftRoom()
    {
        OnRoomLeft?.Invoke();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        OnRoomPlayerLeft?.Invoke(otherPlayer);
    }
}
