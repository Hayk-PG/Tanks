using UnityEngine;
using Photon.Pun;

public class GlobalTileController : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;
    private TilesData _tilesData;
    private ChangeTiles _changeTiles;
    private Transform _parent;
    private PhotonView _photonView;


    private void Awake()
    {
        _tilesData = Get<TilesData>.From(gameObject);
        _changeTiles = Get<ChangeTiles>.From(gameObject);
        _parent = transform;
        _photonView = Get<PhotonView>.From(FindObjectOfType<GameManager>().gameObject);
    }

    public void Modify(Vector3 newTilePosition)
    {
        Conditions<bool>.Compare(MyPhotonNetwork.IsOfflineMode, () => Offline(newTilePosition), () => Online(newTilePosition));
    }

    private void CreateNewTile(Vector3 newTilePosition)
    {
        GameObject tile = Instantiate(_prefab, newTilePosition, Quaternion.identity, _parent);

        if (_tilesData.TilesDict.ContainsKey(newTilePosition))
        {
            Destroy(_tilesData.TilesDict[newTilePosition]);
            _tilesData.TilesDict.Remove(newTilePosition);
        }

        _tilesData.TilesDict.Add(newTilePosition, tile);
        _changeTiles.UpdateTiles(newTilePosition);
    }

    private void Offline(Vector3 newTilePosition)
    {
        CreateNewTile(newTilePosition);
    }

    private void Online(Vector3 newTilePosition)
    {
        _photonView.RPC("CreateNewTile", RpcTarget.AllViaServer, newTilePosition);
    }
}
