using UnityEngine;

public partial class PlayerDeployProps : MonoBehaviour
{
    public virtual void TileProps(bool isPlayer1, Vector3 transformPosition, Vector3 tilePosition)
    {
        TileProps tileProps = Get<TileProps>.From(_tilesData.TilesDict[tilePosition]);
        ActivateTileProps(tileProps, isPlayer1);
    }

    public virtual void TilePropsRPC(bool isPlayer1, Vector3 transformPosition, Vector3 tilePosition)
    {
        if (_photonPlayerDeployRPC == null)
            _photonPlayerDeployRPC = Get<PhotonPlayerDeployPropsRPC>.From(_tankController.BasePlayer.gameObject);

        _photonPlayerDeployRPC?.CallSandBagsRPC(isPlayer1, transformPosition, tilePosition);
    }

    protected virtual void Result(bool isPlayer1, Vector3 transformPosition, Vector3 tilePosition)
    {
        Conditions<bool>.Compare(MyPhotonNetwork.IsOfflineMode, () => TileProps(true, transformPosition, tilePosition), () => TilePropsRPC(isPlayer1, transformPosition, tilePosition));
    }

    protected virtual void OnInstantiate()
    {
        if (_playerTurn.IsMyTurn)
        {
            InstantiateHelper(out bool isPlayer1, out Vector3 transformPosition);

            foreach (var tile in _tilesData.TilesDict)
            {
                if (tile.Value != null)
                {
                    if (IsTileFound(tile.Key.x, isPlayer1, transformPosition) && tile.Value.GetComponent<TileProps>() != null)
                    {
                        if (!tile.Value.GetComponent<Tile>().IsProtected)
                        {
                            Result(isPlayer1, transformPosition, tile.Key);
                            _propsTabCustomization.OnSupportOrPropsChanged?.Invoke(_relatedPropsTypeButton);
                            break;
                        }
                    }
                }
            }
        }
    }
}
