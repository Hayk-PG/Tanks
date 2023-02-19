using UnityEngine;

public class LevelCreator : BaseLevelGenerator
{
    [SerializeField]
    [Space]
    private ColorToPrefab _colorOfArmoredTile;

    [SerializeField]
    [Space]
    private ColorToPrefab _colorOfArmoredWall;

    [SerializeField]
    [Space]
    private ColorToPrefab _colorOfExplosiveBarrel;

    [SerializeField]
    [Space]
    private ColorToPrefab _colorOfMine;

    [SerializeField]
    [Space]
    private ColorToPrefab _colorOfBoostZoneDoubleXp;

    [SerializeField]
    [Space]
    private ColorToPrefab _colorOfBoostZoneSafe;




    protected override void GetLevelGeneratorData(LevelGeneratorData levelGeneratorData)
    {
        CreateArmoredTiles(levelGeneratorData);
        CreateTilesWithArmoredWall(levelGeneratorData);
        CreateMines(levelGeneratorData);
        CreateTiles(levelGeneratorData);
        CreateDoubleXpBoostZone(levelGeneratorData);
        CreateSafeBoostZone(levelGeneratorData);
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
            ActivateProps(Tile(_colorOfArmoredTile._prefab, levelGeneratorData), TileProps.PropsType.MetalGround);
    }

    private void CreateTilesWithArmoredWall(LevelGeneratorData levelGeneratorData)
    {
        if (_colorOfArmoredWall._color == levelGeneratorData.MapTexturePixelColor)
            ActivateProps(Tile(_colorOfArmoredWall._prefab, levelGeneratorData), TileProps.PropsType.MetalGround);
    }

    private void CreateMines(LevelGeneratorData levelGeneratorData)
    {
        if (_colorOfMine._color == levelGeneratorData.MapTexturePixelColor)
            ActivateProps(Tile(_colorOfMine._prefab, levelGeneratorData), TileProps.PropsType.Mine);
    }

    private void CreateDoubleXpBoostZone(LevelGeneratorData levelGeneratorData)
    {
        if (_colorOfBoostZoneDoubleXp._color == levelGeneratorData.MapTexturePixelColor)
        {
            GameObject boostZone = Instantiate(_colorOfBoostZoneDoubleXp._prefab, levelGeneratorData.MapTexturePixelsCoordinate, Quaternion.identity, transform);
            BoostZoneManager boostZoneManager = Get<BoostZoneManager>.From(boostZone);
            boostZoneManager.Init(BoostZoneManager.Feature.XpBoost);
        }
    }

    private void CreateSafeBoostZone(LevelGeneratorData levelGeneratorData)
    {
        if (_colorOfBoostZoneSafe._color == levelGeneratorData.MapTexturePixelColor)
        {
            GameObject boostZone = Instantiate(_colorOfBoostZoneSafe._prefab, levelGeneratorData.MapTexturePixelsCoordinate, Quaternion.identity, transform);
            BoostZoneManager boostZoneManager = Get<BoostZoneManager>.From(boostZone);
            boostZoneManager.Init(BoostZoneManager.Feature.SafeZone);
        }
    }

    private void ActivateProps(GameObject tile, TileProps.PropsType propsType)
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