using UnityEngine;
using Photon.Pun;

public class PhotonPlayerDeployPropsRPC : PhotonPlayerBaseRPC, IPlayerDeployProps
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

    public void ActivateShields(int playerIndex)
    {
        _photonPlayerController.PhotonView.RPC("ActivateShieldsRPC", RpcTarget.AllViaServer, playerIndex);
    }

    [PunRPC]
    private void ActivateShieldsRPC(int playerIndex)
    {
        _photonPlayerTankController?._playerShields.ActivateShields(playerIndex);
    }

    public void ArmoredCubeTileProps(bool isPlayer1, Vector3 transformPosition, Vector3 tilePosition)
    {
        _photonPlayerController.PhotonView.RPC("ArmoredCubeTilePropsRPC", RpcTarget.AllViaServer, isPlayer1, transformPosition, tilePosition);
    }

    [PunRPC]
    private void ArmoredCubeTilePropsRPC(bool isPlayer1, Vector3 transformPosition, Vector3 tilePosition)
    {
        _photonPlayerTankController?._playerDeployMetalCube.TileProps(isPlayer1, transformPosition, tilePosition);
    }

    public void ChangeGroundToArmoredGround(bool isPlayer1, Vector3 transformPosition, Vector3 tilePosition)
    {
        _photonPlayerController.PhotonView.RPC("ChangeGroundToArmoredGroundRPC", RpcTarget.AllViaServer, isPlayer1, transformPosition, tilePosition);
    }

    [PunRPC]
    private void ChangeGroundToArmoredGroundRPC(bool isPlayer1, Vector3 transformPosition, Vector3 tilePosition)
    {
        _photonPlayerTankController?._playerChangeTileToMetalGround.TileProps(isPlayer1, transformPosition, tilePosition);
    }

    public void SkipTurn(TurnState turnState)
    {
        _photonPlayerController.PhotonView.RPC("SkipTurnRPC", RpcTarget.AllViaServer, turnState);
    }

    [PunRPC]
    private void SkipTurnRPC(TurnState turnState)
    {
        _photonPlayerTankController?._playerTankSkipTurn.Skip(turnState);
    }
}
