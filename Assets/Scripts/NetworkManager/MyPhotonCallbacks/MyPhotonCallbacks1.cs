using Photon.Pun;
using System;

public partial class MyPhotonCallbacks : MonoBehaviourPunCallbacks
{
    public Action _OnJoinedLobby { get; set; }

    public override void OnJoinedLobby()
    {
        _OnJoinedLobby?.Invoke();
    }  
}
