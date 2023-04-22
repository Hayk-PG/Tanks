using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class GlobalExplosiveBarrels : MonoBehaviourPun
{
    public void LaunchBarrelRaiseEvent(Vector3 positions)
    {
        if (MyPhotonNetwork.IsMasterClient(MyPhotonNetwork.LocalPlayer))
        {
            EventInfo.Content_LaunchBarrel = new object[] { positions };
            PhotonNetwork.RaiseEvent(EventInfo.Code_LaunchBarrel, EventInfo.Content_LaunchBarrel, new RaiseEventOptions { Receivers = ReceiverGroup.All }, SendOptions.SendUnreliable);
        }
    }

    public void ExplodeBarrelRaiseEvent(string collisionTag, Vector3 collisionPosition, Vector3 id)
    {
        if (MyPhotonNetwork.IsMasterClient(MyPhotonNetwork.LocalPlayer))
        {
            EventInfo.Content_BarrelCollision = new object[] { collisionTag, collisionPosition, id };
            PhotonNetwork.RaiseEvent(EventInfo.Code_BarrelCollision, EventInfo.Content_BarrelCollision, new RaiseEventOptions { Receivers = ReceiverGroup.All }, SendOptions.SendUnreliable);
        }
    }
}
