using UnityEngine;

public interface IPlayerDeployProps
{
    void ActivateShields(int playerIndex);

    void ArmoredCubeTileProps(bool isPlayer1, Vector3 transformPosition, Vector3 tilePosition);

    void ChangeGroundToArmoredGround(bool isPlayer1, Vector3 transformPosition, Vector3 tilePosition);
}
