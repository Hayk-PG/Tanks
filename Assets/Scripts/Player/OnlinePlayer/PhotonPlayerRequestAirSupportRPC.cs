using Photon.Pun;
using UnityEngine;

public class PhotonPlayerRequestAirSupportRPC : PhotonPlayerBaseRPC
{
    public void CallAirSupportRPC(Vector3 position, Quaternion rotation, float distanceX)
    {
        _photonPlayerController.PhotonView.RPC("AirSupportRPC", RpcTarget.AllViaServer, position, rotation, distanceX);
    }

    [PunRPC]
    private void AirSupportRPC(Vector3 position, Quaternion rotation, float distanceX)
    {
        _photonPlayerTankController?._playerRequestAirSupport.RequestAirSupport(position, rotation, distanceX);
    }

    public void CallDropBombRPC()
    {
        _photonPlayerController.PhotonView.RPC("DropBombRPC", RpcTarget.AllViaServer);
    }

    [PunRPC]
    private void DropBombRPC()
    {
        _photonPlayerTankController?._playerRequestAirSupport.DropBomb();
    }
}
