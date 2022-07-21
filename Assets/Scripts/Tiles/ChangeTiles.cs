using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class ChangeTiles : BaseChangeTiles
{
    private float second;
    private IEnumerator _coroutine;

    public event Action<TilesData> OnTilesUpdated;


    protected override void Awake()
    {
        base.Awake();
        _coroutine = UpdateTilesCoroutine(second);
    }

    public void UpdateTiles()
    {
        second = 0;

        StopCoroutine();
        StartCoroutine();

        //StartCoroutine(UpdateTilesCoroutine(second));
    }

    private void StartCoroutine()
    {
        _coroutine = UpdateTilesCoroutine(second);
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

    private IEnumerator UpdateTilesCoroutine(float time)
    {
        while (time < 1)
        {
            time += 1 * Time.deltaTime;

            for (int i = 0; i < TilesGenerator.TilesDict.Keys.ToList().Count; i++)
            {
                if (HasTile(TilesGenerator.TilesDict.Keys.ToList()[i]))
                {
                    ThisTilePos = TilesGenerator.TilesDict.Keys.ToList()[i];

                    if (!HasTile(ThisTilePos + Vertical))
                    {
                        if (HasTile(ThisTilePos - Horizontal) && HasTile(ThisTilePos + Horizontal))
                        {
                            CreateTopTile();
                        }

                        if (HasTile(ThisTilePos - Horizontal) && !HasTile(ThisTilePos + Horizontal) && !HasTile(ThisTilePos - Vertical + Horizontal))
                        {
                            CreateTopRightTile();
                        }

                        if (HasTile(ThisTilePos + Horizontal) && !HasTile(ThisTilePos - Horizontal) && !HasTile(ThisTilePos - Vertical - Horizontal))
                        {
                            CreateTopLeftTile();
                        }

                        if (!HasTile(ThisTilePos - Horizontal) && !HasTile(ThisTilePos + Horizontal))
                        {
                            CreateRightTopLeftTile();
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
                            CreateRightTile();
                        }

                        if (!HasTile(ThisTilePos - Horizontal) && HasTile(ThisTilePos + Horizontal))
                        {
                            CreateLeftTile();
                        }

                        if (!HasTile(ThisTilePos - Horizontal) && !HasTile(ThisTilePos + Horizontal))
                        {
                            CreateLeftRightTile();
                        }

                        if(HasTile(ThisTilePos - Horizontal) && HasTile(ThisTilePos + Horizontal))
                        {
                            CreateMiddleTile();
                        }
                    }
                }
            }

            yield return new WaitForSeconds(0.02f);
        }

        OnTilesUpdated?.Invoke(TilesGenerator);
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
}
