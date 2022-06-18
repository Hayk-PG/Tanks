using UnityEngine;
using Photon.Pun;

public class PhotonPlayerDeployPropsRPC : PhotonPlayerBaseRPC
{
    public void CallSandBagsRPC(bool isPlayer1, Vector3 transformPosition)
    {
        _photonPlayerController.PhotonView.RPC("SandBagsRPC", RpcTarget.AllViaServer, isPlayer1, transformPosition);
    }

    [PunRPC]
    private void SandBagsRPC(bool isPlayer1, Vector3 transformPosition)
    {
        _photonPlayerTankController?._playerDeployProps.Sandbags(isPlayer1, transformPosition);
    }
}
