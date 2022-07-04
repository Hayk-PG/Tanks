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

    public void CalShieldsActivityRPC(int playerIndex)
    {
        _photonPlayerController.PhotonView.RPC("ShieldsActivityRPC", RpcTarget.AllViaServer, playerIndex);
    }

    [PunRPC]
    private void ShieldsActivityRPC(int playerIndex)
    {
        _photonPlayerTankController?._playerShields.ActivateShields(playerIndex);
    }
}
