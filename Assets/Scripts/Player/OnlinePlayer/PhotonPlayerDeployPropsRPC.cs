using UnityEngine;
using Photon.Pun;

public class PhotonPlayerDeployPropsRPC : PhotonPlayerBaseRPC
{
    public void CallSandBagsRPC(bool isPlayer1, Vector3 transformPosition, Vector3 tilePosition)
    {
        _photonPlayerController.PhotonView.RPC("SandBagsRPC", RpcTarget.AllViaServer, isPlayer1, transformPosition, tilePosition);
    }

    [PunRPC]
    private void SandBagsRPC(bool isPlayer1, Vector3 transformPosition, Vector3 tilePosition)
    {
        _photonPlayerTankController?._playerDeployProps.TileProps(isPlayer1, transformPosition, tilePosition);
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
