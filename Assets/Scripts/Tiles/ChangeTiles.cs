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


    private void Awake()
    {
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

    private void CreateTile(Vector3 position)
    {
        if (HasTile(position))
        {
            _thisTilePos = position;

            if (!HasTile(_thisTilePos + Vertical))
            {
                if (HasTile(_thisTilePos - Horizontal) && HasTile(_thisTilePos + Horizontal) && HasTile(_thisTilePos - Vertical))
                {
                    CreateTopTile();
                }

                if (HasTile(_thisTilePos - Horizontal) && HasTile(_thisTilePos + Horizontal) && !HasTile(_thisTilePos - Vertical))
                {
                    CreateTopBottomTile();
                }

                if (!HasTile(_thisTilePos - Horizontal) && HasTile(_thisTilePos + Horizontal) && !HasTile(_thisTilePos - Vertical))
                {
                    CreateTopLeftBottomTile();
                }

                if (HasTile(_thisTilePos - Horizontal) && !HasTile(_thisTilePos + Horizontal) && !HasTile(_thisTilePos - Vertical))
                {
                    CreateTopRightBottomTile();
                }

                if (!HasTile(_thisTilePos - Horizontal) && !HasTile(_thisTilePos + Horizontal) && !HasTile(_thisTilePos - Vertical))
                {
                    CreateRrightTopLeftBottomTile();
                }

                if (!HasTile(_thisTilePos - Horizontal) && !HasTile(_thisTilePos + Horizontal) && HasTile(_thisTilePos - Vertical))
                {
                    CreateRightTopLeftTile();
                }

                if (HasTile(_thisTilePos - Horizontal) && !HasTile(_thisTilePos + Horizontal) && HasTile(_thisTilePos - Vertical) && !HasTile(_thisTilePos - Vertical + Horizontal))
                {
                    //CreateTopRightTile();
                    CreateLeftSlope();
                }

                if (HasTile(_thisTilePos + Horizontal) && !HasTile(_thisTilePos - Horizontal) && HasTile(_thisTilePos - Vertical) && !HasTile(_thisTilePos - Vertical - Horizontal))
                {
                    //CreateTopLeftTile();
                    CreateRightSlope();
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
                if (!HasTile(_thisTilePos - Horizontal) && !HasTile(_thisTilePos + Horizontal) && HasTile(_thisTilePos - Vertical))
                {
                    CreateLeftRightTile();
                }

                if (!HasTile(_thisTilePos - Horizontal) && !HasTile(_thisTilePos + Horizontal) && !HasTile(_thisTilePos - Vertical))
                {
                    CreateRightBottomLeftTile();
                }

                if (HasTile(_thisTilePos - Horizontal) && !HasTile(_thisTilePos + Horizontal) && !HasTile(_thisTilePos - Vertical))
                {
                    CreateBottomRightTile();
                }

                if (!HasTile(_thisTilePos - Horizontal) && HasTile(_thisTilePos + Horizontal) && !HasTile(_thisTilePos - Vertical))
                {
                    CreateBottomLeftTile();
                }

                if (HasTile(_thisTilePos - Horizontal) && !HasTile(_thisTilePos + Horizontal) && HasTile(_thisTilePos - Vertical))
                {
                    CreateRightTile();
                }

                if (!HasTile(_thisTilePos - Horizontal) && HasTile(_thisTilePos + Horizontal) && HasTile(_thisTilePos - Vertical))
                {
                    CreateLeftTile();
                }

                if (HasTile(_thisTilePos - Horizontal) && HasTile(_thisTilePos + Horizontal) && !HasTile(_thisTilePos - Vertical))
                {
                    CreateBottomTile();
                }

                if (HasTile(_thisTilePos - Horizontal) && HasTile(_thisTilePos + Horizontal) && HasTile(_thisTilePos - Vertical))
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
            StoreInOptimizedTilesList(currentTilePosition.Value);
            yield return null;
            CreateTilesFromOptimizedTilesList();
        }
        else
        {
            CreateTilesFromTilesDict();
        }

        OnTilesUpdated?.Invoke(_tileData);
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

    private void CreateBottomTile()
    {
        SetTile(_thisTilePos, _tileData.TilesPrefabs[10]);
    }

    private void CreateBottomLeftTile()
    {
        SetTile(_thisTilePos, _tileData.TilesPrefabs[11]);
    }

    private void CreateBottomRightTile()
    {
        SetTile(_thisTilePos, _tileData.TilesPrefabs[12]);
    }

    private void CreateRightBottomLeftTile()
    {
        SetTile(_thisTilePos, _tileData.TilesPrefabs[13]);
    }

    private void CreateTopLeftBottomTile()
    {
        SetTile(_thisTilePos, _tileData.TilesPrefabs[14]);
    }

    private void CreateTopRightBottomTile()
    {
        SetTile(_thisTilePos, _tileData.TilesPrefabs[15]);
    }

    private void CreateTopBottomTile()
    {
        SetTile(_thisTilePos, _tileData.TilesPrefabs[16]);
    }

    private void CreateRrightTopLeftBottomTile()
    {
        SetTile(_thisTilePos, _tileData.TilesPrefabs[17]);
    }
}
