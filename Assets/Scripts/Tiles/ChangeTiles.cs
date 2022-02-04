using System.Linq;

public class ChangeTiles : BaseChangeTiles
{

    void Update()
    {
        for (int i = 0; i < TilesGenerator.TilesDict.Keys.ToList().Count; i++)
        {
            if (HasTile(TilesGenerator.TilesDict.Keys.ToList()[i]))
            {
                ThisTilePos = TilesGenerator.TilesDict.Keys.ToList()[i];

                if(!HasTile(ThisTilePos + Vertical))
                {
                    if(ThisTilePos.y != TilesGenerator.YStartPoint)
                    {
                        if(HasTile(ThisTilePos - Horizontal) && HasTile(ThisTilePos + Horizontal))
                        {
                            CreateTopTile();
                        }
                    }

                    if(HasTile(ThisTilePos - Horizontal) && !HasTile(ThisTilePos + Horizontal) && !HasTile(ThisTilePos - Vertical + Horizontal))
                    {
                        CreateTopRightTile();
                    }

                    if (HasTile(ThisTilePos + Horizontal) && !HasTile(ThisTilePos - Horizontal) && !HasTile(ThisTilePos - Vertical - Horizontal))
                    {
                        CreateTopLeftTile();
                    }

                    if(!HasTile(ThisTilePos - Horizontal) && !HasTile(ThisTilePos + Horizontal))
                    {
                        CreateRightTopLeftTile();
                    }

                    if(!HasTile(ThisTilePos - Horizontal) && HasTile(ThisTilePos + Horizontal) && HasTile(ThisTilePos - Vertical) && HasTile(ThisTilePos - Vertical - Horizontal))
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
                    if(ThisTilePos.y != TilesGenerator.YStartPoint)
                    {
                        if (HasTile(ThisTilePos - Horizontal) && !HasTile(ThisTilePos + Horizontal))
                        {
                            CreateRightTile();
                        }

                        if(!HasTile(ThisTilePos - Horizontal) && HasTile(ThisTilePos + Horizontal))
                        {
                            CreateLeftTile();
                        }

                        if (!HasTile(ThisTilePos - Horizontal) && !HasTile(ThisTilePos + Horizontal))
                        {
                            CreateLeftRightTile();
                        }
                    }
                }
            }
        }
    }

    void CreateTopTile()
    {
        SetTile(ThisTilePos, TilesGenerator.TilesPrefabs[0]);
    }

    void CreateTopRightTile()
    {
        SetTile(ThisTilePos, TilesGenerator.TilesPrefabs[5]);
    }

    void CreateTopLeftTile()
    {
        SetTile(ThisTilePos, TilesGenerator.TilesPrefabs[4]);
    }

    void CreateRightTile()
    {
        SetTile(ThisTilePos, TilesGenerator.TilesPrefabs[3]);
    }

    void CreateLeftTile()
    {
        SetTile(ThisTilePos, TilesGenerator.TilesPrefabs[2]);
    }

    void CreateRightTopLeftTile()
    {
        SetTile(ThisTilePos, TilesGenerator.TilesPrefabs[8]);
    }

    void CreateLeftRightTile()
    {
        SetTile(ThisTilePos, TilesGenerator.TilesPrefabs[9]);
    }

    void CreateRightSlope()
    {
        SetTile(ThisTilePos, TilesGenerator.TilesPrefabs[7]);
    }

    void CreateLeftSlope()
    {
        SetTile(ThisTilePos, TilesGenerator.TilesPrefabs[6]);
    }
}
