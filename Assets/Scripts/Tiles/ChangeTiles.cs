using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChangeTiles : BaseChangeTiles
{
    private List<Vector3> _optimizedTilesList;
    private float second;
    private IEnumerator _coroutine;

    public event Action<TilesData> OnTilesUpdated;


    protected override void Awake()
    {
        base.Awake();
        _coroutine = UpdateTilesCoroutine(second, null);
    }

    public void UpdateTiles(Vector3? currentTilePosition)
    {
        second = 0;

        StopCoroutine();
        StartCoroutine(currentTilePosition);
    }

    private void StartCoroutine(Vector3? currentTilePosition)
    {
        _coroutine = UpdateTilesCoroutine(second, currentTilePosition);
        StartCoroutine(_coroutine);
    }

    private void StopCoroutine()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
        }
    }
   
    private void CreateTopTile()
    {
        SetTile(ThisTilePos, TilesGenerator.TilesPrefabs[0]);
    }

    private void CreateTopRightTile()
    {
        SetTile(ThisTilePos, TilesGenerator.TilesPrefabs[5]);
    }

    private void CreateTopLeftTile()
    {
        SetTile(ThisTilePos, TilesGenerator.TilesPrefabs[4]);
    }

    private void CreateRightTile()
    {
        SetTile(ThisTilePos, TilesGenerator.TilesPrefabs[3]);
    }

    private void CreateLeftTile()
    {
        SetTile(ThisTilePos, TilesGenerator.TilesPrefabs[2]);
    }

    private void CreateRightTopLeftTile()
    {
        SetTile(ThisTilePos, TilesGenerator.TilesPrefabs[8]);
    }

    private void CreateLeftRightTile()
    {
        SetTile(ThisTilePos, TilesGenerator.TilesPrefabs[9]);
    }

    private void CreateRightSlope()
    {
        SetTile(ThisTilePos, TilesGenerator.TilesPrefabs[7]);
    }

    private void CreateLeftSlope()
    {
        SetTile(ThisTilePos, TilesGenerator.TilesPrefabs[6]);
    }

    private void CreateMiddleTile()
    {
        SetTile(ThisTilePos, TilesGenerator.TilesPrefabs[1]);
    }

    private void CreateTile(Vector3 position)
    {
        if (HasTile(position))
        {
            ThisTilePos = position;

            if (!HasTile(ThisTilePos + Vertical))
            {
                if (HasTile(ThisTilePos - Horizontal) && HasTile(ThisTilePos + Horizontal))
                {
                    CreateTopTile();
                }

                if (HasTile(ThisTilePos - Horizontal) && !HasTile(ThisTilePos + Horizontal) && !HasTile(ThisTilePos - Vertical + Horizontal))
                {
                    if (position.x != LevelGenerator.MapHorizontalEndPoint)
                        CreateTopRightTile();
                    else
                        CreateTopTile();                   
                }

                if (HasTile(ThisTilePos + Horizontal) && !HasTile(ThisTilePos - Horizontal) && !HasTile(ThisTilePos - Vertical - Horizontal))
                {
                    if (position.x != LevelGenerator.MapHorizontalStartPoint)
                        CreateTopLeftTile();
                    else
                        CreateTopTile(); 

                }

                if (!HasTile(ThisTilePos - Horizontal) && !HasTile(ThisTilePos + Horizontal))
                {
                    if (position.x != LevelGenerator.MapHorizontalStartPoint && position.x != LevelGenerator.MapHorizontalEndPoint)
                        CreateRightTopLeftTile();

                    if (position.x == LevelGenerator.MapHorizontalStartPoint)
                        CreateTopRightTile();

                    if (position.x == LevelGenerator.MapHorizontalEndPoint)
                        CreateTopLeftTile();
                }

                if (!HasTile(ThisTilePos - Horizontal) && HasTile(ThisTilePos + Horizontal) && HasTile(ThisTilePos - Vertical) && HasTile(ThisTilePos - Vertical - Horizontal))
                {
                    CreateRightSlope();
                }

                if (!HasTile(ThisTilePos + Horizontal) && HasTile(ThisTilePos - Horizontal) && HasTile(ThisTilePos - Vertical) && HasTile(ThisTilePos - Vertical + Horizontal))
                {
                    CreateLeftSlope();
                }
            }
            else
            {
                if (HasTile(ThisTilePos - Horizontal) && !HasTile(ThisTilePos + Horizontal))
                {
                    if (position.x != LevelGenerator.MapHorizontalEndPoint)
                        CreateRightTile();
                    else
                        CreateMiddleTile();                   
                }

                if (!HasTile(ThisTilePos - Horizontal) && HasTile(ThisTilePos + Horizontal))
                {
                    if (position.x != LevelGenerator.MapHorizontalStartPoint)
                        CreateLeftTile();
                    else
                        CreateMiddleTile();                   
                }

                if (!HasTile(ThisTilePos - Horizontal) && !HasTile(ThisTilePos + Horizontal))
                {
                    CreateLeftRightTile();
                }

                if (HasTile(ThisTilePos - Horizontal) && HasTile(ThisTilePos + Horizontal))
                {
                    CreateMiddleTile();
                }
            }
        }
    }

    private void StoreInOptimizedTilesList(Vector3 currentTilePosition)
    {
        _optimizedTilesList = new List<Vector3>();

        foreach (var tile in TilesGenerator.TilesDict)
        {
            if (tile.Key == currentTilePosition || tile.Key.x >= currentTilePosition.x - 1 && tile.Key.x <= currentTilePosition.x + 1)
            {
                _optimizedTilesList.Add(tile.Key);
            }
        }
    }

    private void CreateTilesFromOptimizedTilesList()
    {
        for (int i = 0; i < _optimizedTilesList.Count; i++)
        {
            CreateTile(_optimizedTilesList[i]);
        }
    }

    private void CreateTilesFromTilesDict()
    {
        for (int i = 0; i < TilesGenerator.TilesDict.Keys.ToList().Count; i++)
        {
            if (HasTile(TilesGenerator.TilesDict.Keys.ToList()[i]))
            {
                ThisTilePos = TilesGenerator.TilesDict.Keys.ToList()[i];
                CreateTile(ThisTilePos);
            }
        }
    }

    private IEnumerator UpdateTilesCoroutine(float time, Vector3? currentTilePosition)
    {
        if (currentTilePosition != null)
        {
            StoreInOptimizedTilesList(currentTilePosition.Value);

            yield return null;

            while (time < 1)
            {
                time += 1 * Time.deltaTime;
                CreateTilesFromOptimizedTilesList();
                yield return new WaitForSeconds(0.02f);
            }
        }
        else
        {
            while (time < 1)
            {
                time += 1 * Time.deltaTime;
                CreateTilesFromTilesDict();
                yield return new WaitForSeconds(0.02f);
            }
        }

        OnTilesUpdated?.Invoke(TilesGenerator);
    }
}
