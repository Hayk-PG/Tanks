using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseChangeTiles : MonoBehaviour
{
    protected TilesGenerator TilesGenerator { get; set; }

    protected Vector3 ThisTilePos;

    protected Vector3 Vertical { get => new Vector3(0, TilesGenerator.Size, 0); }
    protected Vector3 Horizontal { get => new Vector3(TilesGenerator.Size, 0, 0); }


    protected virtual void Awake()
    {
        TilesGenerator = GetComponent<TilesGenerator>();
    }

    protected bool HasTile(Vector3 pos)
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

    void OnTileUpdates(Vector3 pos, GameObject tile)
    {
        Destroy(TilesGenerator.TilesDict[pos]);
        TilesGenerator.TilesDict.Remove(pos);
        GameObject newTile = Instantiate(tile, pos, Quaternion.identity);
        newTile.name = tile.name;
        TilesGenerator.TilesDict.Add(pos, newTile);
        print(TilesGenerator.TilesDict[pos] + "/" + tile + "/" + TilesGenerator.TilesDict[pos].transform.position);
    }

    protected void SetTile(Vector3 pos, GameObject tile)
    {
        if(tile != null)
        {
            UpdateTile(pos, tile);
        }
    }
}
