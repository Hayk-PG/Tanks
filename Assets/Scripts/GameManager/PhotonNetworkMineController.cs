using UnityEngine;
using Photon.Pun;

public class PhotonNetworkMineController : MonoBehaviourPun
{
    public void TriggerMine(Vector3 id)
    {
        if (MyPhotonNetwork.IsMasterClient(MyPhotonNetwork.LocalPlayer))
            photonView.RPC("TriggerMineRPC", RpcTarget.AllViaServer, id);
    }

    [PunRPC]
    private void TriggerMineRPC(Vector3 id)
    {
        Mine mine = GlobalFunctions.ObjectsOfType<Mine>.Find(m => m.ID == id);
        mine?.TriggerMine();
    }
}
