using Photon.Pun;


public class PhotonPlayerJumpExecuterRPC : PhotonPlayerBaseRPC, IPhotonPlayerJumpExecuter
{
    public void Jump()
    {
        _photonPlayerController.PhotonView.RPC("JumpRPC", RpcTarget.Others);
    }

    [PunRPC]
    private void JumpRPC()
    {
        _photonPlayerTankController._tankMovement.Jump();
    }
}
