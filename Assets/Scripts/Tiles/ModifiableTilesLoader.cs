using UnityEngine;


public class ModifiableTilesLoader : MonoBehaviour
{
    private bool _isLoading;

    public GameObject Container { get; private set; }



    private void Start()
    {
        LoadAssetAsync();
    }

    private void LoadAssetAsync()
    {
        MyAddressable.LoadAssetAsync((string)AddressablesPath.ModifiableTiles[0, 0], (int)AddressablesPath.ModifiableTiles[0, 1], true, InstantiateTiles, null);
    }

    private void InstantiateTiles(GameObject gameObject)
    {
        Container = Instantiate(gameObject);
        _isLoading = false;
    }

    public void TrackTilesCount()
    {
        if (Container.transform.childCount == 0)
        {
            Destroy(Container);

            if (!_isLoading)
            {
                print("Loading new assets");
                LoadAssetAsync();
                _isLoading = true;
            }
        }
    }
}
