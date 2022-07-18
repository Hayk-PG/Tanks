using UnityEngine;

public partial class PlayerDeployProps : MonoBehaviour
{
    private bool IsTileFound(float tilePosX, bool isPlayer1, Vector3 transformPosition)
    {
        return isPlayer1 ? tilePosX >= transformPosition.x + 0.5f && tilePosX <= transformPosition.x + 1.5f :
                           tilePosX <= transformPosition.x - 0.5f && tilePosX >= transformPosition.x - 1.5f;
    }
    public void Sandbags(bool isPlayer1, Vector3 transformPosition, Vector3 tilePosition)
    {
        TileProps tileProps = Get<TileProps>.From(_tilesData.TilesDict[tilePosition]);
        tileProps?.OnSandbags(true, isPlayer1);
    }

    private void SandbagsRPC(bool isPlayer1, Vector3 transformPosition, Vector3 tilePosition)
    {
        if (_photonPlayerDeployRPC == null)
            _photonPlayerDeployRPC = Get<PhotonPlayerDeployPropsRPC>.From(_tankController.BasePlayer.gameObject);

        _photonPlayerDeployRPC?.CallSandBagsRPC(isPlayer1, transformPosition, tilePosition);
    }

    private void OnInstantiateSandbags()
    {
        if (_playerTurn.IsMyTurn)
        {
            bool isPlayer1 = _playerTurn.MyTurn == TurnState.Player1;
            Vector3 transformPosition = transform.position;

            foreach (var tile in _tilesData.TilesDict)
            {
                if (IsTileFound(tile.Key.x, isPlayer1, transformPosition) && tile.Value.GetComponent<TileProps>() != null)
                {
                    if (!tile.Value.GetComponent<Tile>().HasSandbagsOnIt)
                    {
                        Conditions<bool>.Compare(MyPhotonNetwork.IsOfflineMode, () => Sandbags(true, transformPosition, tile.Key), () => SandbagsRPC(isPlayer1, transformPosition, tile.Key));
                        _propsTabCustomization.OnSupportOrPropsChanged?.Invoke(_relatedPropsTypeButton);
                        break;
                    }
                }
            }
        }
    }
}
