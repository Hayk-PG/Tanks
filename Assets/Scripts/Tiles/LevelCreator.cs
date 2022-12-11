using UnityEngine;

public class LevelCreator : BaseLevelGenerator
{
    [SerializeField] private ColorToPrefab _colorOfArmoredTile;
    [SerializeField] private ColorToPrefab _colorOfArmoredWall;

    protected override void GetLevelGeneratorData(LevelGeneratorData levelGeneratorData)
    {
        CreateArmoredTiles(levelGeneratorData);
        CreateTilesWithArmoredWall(levelGeneratorData);
        CreateTiles(levelGeneratorData);
    }

    private void CreateTiles(LevelGeneratorData levelGeneratorData)
    {
        GlobalFunctions.Loop<ColorToPrefab>.Foreach(_colorToPrefabs, colorToPrefab =>
        {
            if (colorToPrefab._color == levelGeneratorData.MapTexturePixelColor)
                Tile(colorToPrefab._prefab, levelGeneratorData);
        });
    }

    private void CreateArmoredTiles(LevelGeneratorData levelGeneratorData)
    {
        if (_colorOfArmoredTile._color == levelGeneratorData.MapTexturePixelColor)
            AcitavateProps(Tile(_colorOfArmoredTile._prefab, levelGeneratorData), TileProps.PropsType.MetalGround);
    }

    private void CreateTilesWithArmoredWall(LevelGeneratorData levelGeneratorData)
    {
        if (_colorOfArmoredWall._color == levelGeneratorData.MapTexturePixelColor)
            AcitavateProps(Tile(_colorOfArmoredWall._prefab, levelGeneratorData), TileProps.PropsType.MetalGround);
    }

    private void AcitavateProps(GameObject tile, TileProps.PropsType propsType)
    {
        TileProps tileProps = Get<TileProps>.From(tile);
        tileProps?.ActiveProps(propsType, true, null);
    }

    private GameObject Tile(GameObject tile, LevelGeneratorData levelGeneratorData)
    {
        GameObject newTile = Instantiate(tile, levelGeneratorData.MapTexturePixelsCoordinate, Quaternion.identity, transform);
        _tilesData.TilesDict.Add(levelGeneratorData.MapTexturePixelsCoordinate, newTile);
        return newTile;
    }
}
