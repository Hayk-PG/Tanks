using Photon.Pun;
using System;

public partial class MyPhotonCallbacks : MonoBehaviourPunCallbacks
{
    public Action OnPhotonJoinedLobby { get; set; }

    public override void OnJoinedLobby()
    {
        OnPhotonJoinedLobby?.Invoke();
    }  
}
