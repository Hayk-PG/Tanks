using UnityEngine;

public class PlatformPoints : BaseLevelGenerator
{
    [SerializeField] Platform _platformHorizontal;
    [SerializeField] Platform _platformVertical;

    private Vector3? _horizontalPlatformStart, _horizontalPlatformEnd;
    private Vector3? _verticalPlatformStart, _verticalPlatformEnd;


    protected override void GetLevelGeneratorData(LevelGeneratorData levelGeneratorData)
    {
        Get(levelGeneratorData);
        Set();
    }

    private void Get(LevelGeneratorData levelGeneratorData)
    {
        if (levelGeneratorData.MapTexturePixelColor == _colorToPrefabs[0]._color)
            _horizontalPlatformStart = levelGeneratorData.MapTexturePixelsCoordinate;

        if (levelGeneratorData.MapTexturePixelColor == _colorToPrefabs[1]._color)
            _horizontalPlatformEnd = levelGeneratorData.MapTexturePixelsCoordinate;

        if (levelGeneratorData.MapTexturePixelColor == _colorToPrefabs[2]._color)
            _verticalPlatformStart = levelGeneratorData.MapTexturePixelsCoordinate;

        if (levelGeneratorData.MapTexturePixelColor == _colorToPrefabs[3]._color)
            _verticalPlatformEnd = levelGeneratorData.MapTexturePixelsCoordinate;
    }

    private void Set()
    {
        if (_horizontalPlatformStart.HasValue && _horizontalPlatformEnd.HasValue)
        {
            _platformHorizontal.gameObject.SetActive(true);
            _platformHorizontal.transform.position = _horizontalPlatformStart.Value;
            _platformHorizontal.Set(_horizontalPlatformStart.Value, _horizontalPlatformEnd.Value);
        }

        if (_verticalPlatformStart.HasValue && _verticalPlatformEnd.HasValue)
        {
            _platformVertical.gameObject.SetActive(true);
            _platformVertical.transform.position = _verticalPlatformStart.Value;
            _platformVertical.Set(_verticalPlatformStart.Value, _verticalPlatformEnd.Value);
        }
    }
}
