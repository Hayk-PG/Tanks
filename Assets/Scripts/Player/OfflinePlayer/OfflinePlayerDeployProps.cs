using UnityEngine;

public class OfflinePlayerDeployProps : OfflinePlayerBase, IPlayerDeployProps
{
    public void ActivateShields(int playerIndex)
    {
        _offlinePlayerTankController?._playerShields.ActivateShields(playerIndex);
    }

    public void ArmoredCubeTileProps(bool isPlayer1, Vector3 transformPosition, Vector3 tilePosition)
    {
        _offlinePlayerTankController?._playerDeployMetalCube.TileProps(isPlayer1, transformPosition, tilePosition);
    }

    public void ChangeGroundToArmoredGround(bool isPlayer1, Vector3 transformPosition, Vector3 tilePosition)
    {
        _offlinePlayerTankController?._playerChangeTileToMetalGround.TileProps(isPlayer1, transformPosition, tilePosition);
    }

    public void SkipTurn(TurnState turnState)
    {
        _offlinePlayerTankController?._playerTankSkipTurn.Skip(turnState);
    }
}
