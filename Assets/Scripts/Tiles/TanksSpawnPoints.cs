public class TanksSpawnPoints : BaseLevelGenerator
{
    protected override void GetLevelGeneratorData(LevelGeneratorData levelGeneratorData) => Set(levelGeneratorData);

    private void Set(LevelGeneratorData levelGeneratorData)
    {
        if (_colorToPrefabs[0]._color == levelGeneratorData.MapTexturePixelColor)
            _colorToPrefabs[0]._prefab.transform.position = levelGeneratorData.MapTexturePixelsCoordinate;

        if (_colorToPrefabs[1]._color == levelGeneratorData.MapTexturePixelColor)
            _colorToPrefabs[1]._prefab.transform.position = levelGeneratorData.MapTexturePixelsCoordinate;
    }
}
