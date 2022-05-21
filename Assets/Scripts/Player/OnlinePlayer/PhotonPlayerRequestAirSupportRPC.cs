using Photon.Pun;

public class PhotonPlayerRequestAirSupportRPC : PhotonPlayerBaseRPC
{
    public void CallAirSupportRPC()
    {
        _photonPlayerController.PhotonView.RPC("AirSupportRPC", RpcTarget.AllViaServer);
    }

    [PunRPC]
    private void AirSupportRPC()
    {
        _photonPlayerTankController?._playerRequestAirSupport.RequestAirSupport();
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
