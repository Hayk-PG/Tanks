using Photon.Pun;
using Photon.Realtime;
using System;

public partial class MyPhotonCallbacks : MonoBehaviourPunCallbacks
{
    public Action _OnLeftLobby { get; set; }
    public Action _OnLeftRoom { get; set; }
    public Action<Player> _OnPlayerLeftRoom { get; set; }


    public override void OnLeftLobby()
    {
        _OnLeftLobby?.Invoke();
    }

    public override void OnLeftRoom()
    {
        _OnLeftRoom?.Invoke();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        _OnPlayerLeftRoom?.Invoke(otherPlayer);
    }
}
