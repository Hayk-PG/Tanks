using UnityEngine;

public class AALauncherContent : IWoodBoxContent
{
    private TilesData _tilesData;
    private PhotonNetworkAALauncher _photonNetworkAALaucnher;


    public void Use(TankController tankController, Tab_WoodboxContent tab_WoodboxContent)
    {
        ActivateAALauncher(tankController);
    }

    private void ActivateAALauncher(TankController tankController)
    {
        _tilesData = MonoBehaviour.FindObjectOfType<TilesData>();
        _photonNetworkAALaucnher = MonoBehaviour.FindObjectOfType<PhotonNetworkAALauncher>();
        int maxDistance = 1;

        foreach (var tileDict in _tilesData.TilesDict)
        {
            float distance = Vector3.Distance(tileDict.Key, tankController.transform.position);

            if (distance <= maxDistance && maxDistance <= 6)
            {
                Tile tile = Get<Tile>.From(tileDict.Value);
                TileProps tileProps = Get<TileProps>.FromChild(tileDict.Value);

                if (!tile.IsProtected && tileProps != null)
                {
                    Conditions<bool>.Compare(MyPhotonNetwork.IsOfflineMode, () => Activate(tileProps, tankController), () => Activate(_photonNetworkAALaucnher, tileDict.Key, tankController.gameObject.name));
                    break;
                }
                else
                {
                    maxDistance++;
                }
            }
        }
    }

    private void Activate(TileProps tileProps, TankController tankController)
    {
        tileProps.ActiveProps(TileProps.PropsType.AALauncher, true, tankController.gameObject.name == Names.Tank_FirstPlayer);
    }

    private void Activate(PhotonNetworkAALauncher photonNetworkAALauncher, Vector3 tilePosition, string tankName)
    {
        photonNetworkAALauncher.ActivateAALauncher(tilePosition, tankName);
    }
}
