public class MapPoints : BaseLevelGenerator
{
    public float HorizontalMin { get; private set; }
    public float HorizontalMax { get; private set; }


    protected override void GetLevelGeneratorData(LevelGeneratorData levelGeneratorData) => Set(levelGeneratorData);

    private void Set(LevelGeneratorData levelGeneratorData)
    {
        if (levelGeneratorData.MepTextureDimension.x == 0)
            HorizontalMin = levelGeneratorData.MapTexturePixelsCoordinate.x;

        if (levelGeneratorData.MepTextureDimension.x == levelGeneratorData.MapTextureWidth - 1)
            HorizontalMax = levelGeneratorData.MapTexturePixelsCoordinate.x;
    }
}
