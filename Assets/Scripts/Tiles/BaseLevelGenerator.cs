using UnityEngine;

public abstract class BaseLevelGenerator : MonoBehaviour
{
    [SerializeField] protected ColorToPrefab[] _colorToPrefabs;
    protected LevelGenerator _levelGenerator;
    protected TilesData _tilesData;



    protected virtual void Awake()
    {
        _levelGenerator = Get<LevelGenerator>.From(gameObject);
        _tilesData = Get<TilesData>.From(gameObject);
    }

    protected virtual void OnEnable() => _levelGenerator.onMapTexturePixels += GetLevelGeneratorData;

    protected virtual void OnDisable() => _levelGenerator.onMapTexturePixels -= GetLevelGeneratorData;

    protected abstract void GetLevelGeneratorData(LevelGeneratorData levelGeneratorData);

    protected virtual bool IsColorInSpecificRange(Color color1, Color color2)
    {
        bool r = color1.r >= color2.r - 0.005f && color1.r <= color2.r + 0.005f;
        bool b = color1.b >= color2.b - 0.005f && color1.b <= color2.b + 0.005f;
        bool g = color1.g >= color2.g - 0.005f && color1.g <= color2.g + 0.005f;
        bool a = color1.a >= color2.a - 0.005f && color1.a <= color2.a + 0.005f;

        return r && b && g && a;
    }
}
