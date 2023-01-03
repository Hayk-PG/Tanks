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
}
