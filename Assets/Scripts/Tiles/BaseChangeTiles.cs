using UnityEngine;

public class BaseChangeTiles : MonoBehaviour
{
    protected TilesData TilesGenerator { get; set; }

    protected Vector3 ThisTilePos;

    public Vector3 Vertical { get => new Vector3(0, TilesGenerator.Size, 0); }
    public Vector3 Horizontal { get => new Vector3(TilesGenerator.Size, 0, 0); }


    protected virtual void Awake()
    {
        TilesGenerator = GetComponent<TilesData>();
    }

    public bool HasTile(Vector3 pos)
    {
        return TilesGenerator.TilesDict.ContainsKey(pos) && TilesGenerator.TilesDict[pos].gameObject != null ? true :
               TilesGenerator.TilesDict.ContainsKey(pos) && TilesGenerator.TilesDict[pos].gameObject == null ? false :
               !TilesGenerator.TilesDict.ContainsKey(pos) ? false : false;

    }

    protected void UpdateTile(Vector3 pos, GameObject tile)
    {
        if(TilesGenerator.TilesDict[pos].name != tile.name)
        {
            OnTileUpdates(pos, tile);
        }
    }

    private void OnTileUpdates(Vector3 pos, GameObject tile)
    {
        Destroy(TilesGenerator.TilesDict[pos]);
        TilesGenerator.TilesDict.Remove(pos);
        GameObject newTile = Instantiate(tile, pos, Quaternion.identity);
        newTile.name = tile.name;
        TilesGenerator.TilesDict.Add(pos, newTile);
        newTile.transform.SetParent(transform);
    }

    protected void SetTile(Vector3 pos, GameObject tile)
    {
        if(tile != null)
        {
            UpdateTile(pos, tile);
        }
    }
}
