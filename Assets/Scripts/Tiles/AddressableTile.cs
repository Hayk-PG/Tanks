using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;



[Serializable]
public class AssetReferenceTiles: AssetReferenceT<Tile>
{
    public AssetReferenceTiles(string guid) : base(guid)
    {

    }
}

//ADDRESSABLE
public class AddressableTile : MonoBehaviour
{
    public static AddressableTile Loader { get; private set; }

    [SerializeField]
    private List<AssetReference> _assetReferenceTilesMesh;

    public List<AssetReference> TilesMesh => _assetReferenceTilesMesh;




    private void Awake()
    {
        if(Loader == null)
        {
            Loader = this;

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start() => LoadAssets();

    private void LoadAssets()
    {
        foreach (var asset in _assetReferenceTilesMesh)
        {
            if (asset.OperationHandle.IsValid())
                continue;

            asset.LoadAssetAsync<GameObject>();
        }
    }
}
