using Photon.Pun;

public class PhotonPlayerEnableHUDRPC : PhotonPlayerBaseRPC
{
    public void CallHUDRPC(bool isActive)
    {
        _photonPlayerController.PhotonView.RPC("EnableHUDRPC", RpcTarget.AllViaServer, isActive);
    }

    [PunRPC]
    private void EnableHUDRPC(bool isActive)
    {
        _photonPlayerTankController?._playerHud.MainCanvasGroupActivity(isActive);
    }
}
