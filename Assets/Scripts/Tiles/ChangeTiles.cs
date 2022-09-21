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
        SetTile(_thisTilePos, _tileData.TilesPrefabs[0]);
    }

    private void CreateTopRightTile()
    {
        SetTile(_thisTilePos, _tileData.TilesPrefabs[5]);
    }

    private void CreateTopLeftTile()
    {
        SetTile(_thisTilePos, _tileData.TilesPrefabs[4]);
    }

    private void CreateRightTile()
    {
        SetTile(_thisTilePos, _tileData.TilesPrefabs[3]);
    }

    private void CreateLeftTile()
    {
        SetTile(_thisTilePos, _tileData.TilesPrefabs[2]);
    }

    private void CreateRightTopLeftTile()
    {
        SetTile(_thisTilePos, _tileData.TilesPrefabs[8]);
    }

    private void CreateLeftRightTile()
    {
        SetTile(_thisTilePos, _tileData.TilesPrefabs[9]);
    }

    private void CreateRightSlope()
    {
        SetTile(_thisTilePos, _tileData.TilesPrefabs[7]);
    }

    private void CreateLeftSlope()
    {
        SetTile(_thisTilePos, _tileData.TilesPrefabs[6]);
    }

    private void CreateMiddleTile()
    {
        SetTile(_thisTilePos, _tileData.TilesPrefabs[1]);
    }

    private void CreateTile(Vector3 position)
    {
        if (HasTile(position))
        {
            _thisTilePos = position;

            if (!HasTile(_thisTilePos + Vertical))
            {
                if (HasTile(_thisTilePos - Horizontal) && HasTile(_thisTilePos + Horizontal))
                {
                    CreateTopTile();
                }

                if (HasTile(_thisTilePos - Horizontal) && !HasTile(_thisTilePos + Horizontal) && !HasTile(_thisTilePos - Vertical + Horizontal))
                {
                    if (position.x != _levelGenerator.MapHorizontalEndPoint)
                        CreateTopRightTile();
                    else
                        CreateTopTile();                   
                }

                if (HasTile(_thisTilePos + Horizontal) && !HasTile(_thisTilePos - Horizontal) && !HasTile(_thisTilePos - Vertical - Horizontal))
                {
                    if (position.x != _levelGenerator.MapHorizontalStartPoint)
                        CreateTopLeftTile();
                    else
                        CreateTopTile(); 

                }

                if (!HasTile(_thisTilePos - Horizontal) && !HasTile(_thisTilePos + Horizontal))
                {
                    if (position.x != _levelGenerator.MapHorizontalStartPoint && position.x != _levelGenerator.MapHorizontalEndPoint)
                        CreateRightTopLeftTile();

                    if (position.x == _levelGenerator.MapHorizontalStartPoint)
                        CreateTopRightTile();

                    if (position.x == _levelGenerator.MapHorizontalEndPoint)
                        CreateTopLeftTile();
                }

                if (!HasTile(_thisTilePos - Horizontal) && HasTile(_thisTilePos + Horizontal) && HasTile(_thisTilePos - Vertical) && HasTile(_thisTilePos - Vertical - Horizontal))
                {
                    CreateRightSlope();
                }

                if (!HasTile(_thisTilePos + Horizontal) && HasTile(_thisTilePos - Horizontal) && HasTile(_thisTilePos - Vertical) && HasTile(_thisTilePos - Vertical + Horizontal))
                {
                    CreateLeftSlope();
                }
            }
            else
            {
                if (HasTile(_thisTilePos - Horizontal) && !HasTile(_thisTilePos + Horizontal))
                {
                    if (position.x != _levelGenerator.MapHorizontalEndPoint)
                        CreateRightTile();
                    else
                        CreateMiddleTile();                   
                }

                if (!HasTile(_thisTilePos - Horizontal) && HasTile(_thisTilePos + Horizontal))
                {
                    if (position.x != _levelGenerator.MapHorizontalStartPoint)
                        CreateLeftTile();
                    else
                        CreateMiddleTile();                   
                }

                if (!HasTile(_thisTilePos - Horizontal) && !HasTile(_thisTilePos + Horizontal))
                {
                    CreateLeftRightTile();
                }

                if (HasTile(_thisTilePos - Horizontal) && HasTile(_thisTilePos + Horizontal))
                {
                    CreateMiddleTile();
                }
            }
        }
    }

    private void StoreInOptimizedTilesList(Vector3 currentTilePosition)
    {
        _optimizedTilesList = new List<Vector3>();

        foreach (var tile in _tileData.TilesDict)
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
        for (int i = 0; i < _tileData.TilesDict.Keys.ToList().Count; i++)
        {
            if (HasTile(_tileData.TilesDict.Keys.ToList()[i]))
            {
                _thisTilePos = _tileData.TilesDict.Keys.ToList()[i];
                CreateTile(_thisTilePos);
            }
        }
    }

    private IEnumerator UpdateTilesCoroutine(float time, Vector3? currentTilePosition)
    {
        if (currentTilePosition != null)
        {
            _tileCreationMode = TileCreationMode.Pool;
            StoreInOptimizedTilesList(currentTilePosition.Value);
            yield return null;
            CreateTilesFromOptimizedTilesList();
        }
        else
        {
            _tileCreationMode = TileCreationMode.Instantiate;
            CreateTilesFromTilesDict();
        }

        OnTilesUpdated?.Invoke(_tileData);
    }
}
