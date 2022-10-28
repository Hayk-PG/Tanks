using Photon.Pun;
using Photon.Realtime;
using System;

public partial class MyPhotonCallbacks : MonoBehaviourPunCallbacks
{
    public Action _OnConnectedToMaster { get; set; }
    public event Action onDisconnect;


    public override void OnConnectedToMaster()
    {
        _OnConnectedToMaster?.Invoke();          
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        onDisconnect?.Invoke();
        GlobalFunctions.DebugLog("Disconnect");
    }
}
