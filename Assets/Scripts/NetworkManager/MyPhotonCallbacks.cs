using Photon.Pun;
using Photon.Realtime;
using System;

public partial class MyPhotonCallbacks : MonoBehaviourPunCallbacks
{
    public Action OnPhotonConnectedToMaster { get; set; }


    public override void OnConnectedToMaster()
    {
        OnPhotonConnectedToMaster?.Invoke();          
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        print("Disconnect");
    }
}
