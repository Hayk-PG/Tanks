using UnityEngine;

public partial class PlayerDeployProps : MonoBehaviour
{
    public void Sandbags(bool isPlayer1, Vector3 transformPosition)
    {
        foreach (var tile in _tilesData.TilesDict)
        {
            if (IsTileFound(tile.Key.x, isPlayer1, transformPosition) && tile.Value.GetComponent<TileProps>() != null)
            {
                TileProps tileProps = tile.Value.GetComponent<TileProps>();
                tileProps?.OnSandbags(true, isPlayer1);
                break;
            }
        }
    }

    private void SandbagsRPC(bool isPlayer1, Vector3 transformPosition)
    {
        if (_photonPlayerDeployRPC == null)
            _photonPlayerDeployRPC = Get<PhotonPlayerDeployPropsRPC>.From(_tankController.BasePlayer.gameObject);

        _photonPlayerDeployRPC?.CallSandBagsRPC(isPlayer1, transformPosition);
    }

    private bool IsTileFound(float tilePosX, bool isPlayer1, Vector3 transformPosition)
    {
        return isPlayer1 ? tilePosX >= transformPosition.x + 0.5f && tilePosX <= transformPosition.x + 1.5f:
                           tilePosX <= transformPosition.x - 0.5f && tilePosX >= transformPosition.x - 1.5f;
    }

    private void OnInstantiateSandbags()
    {
        if (_playerTurn.IsMyTurn)
        {
            bool isPlayer1 = _playerTurn.MyTurn == TurnState.Player1;
            Vector3 transformPosition = transform.position;
            Conditions<bool>.Compare(MyPhotonNetwork.IsOfflineMode, () => Sandbags(true, transformPosition), () => SandbagsRPC(isPlayer1, transformPosition));
        }
    }
}
