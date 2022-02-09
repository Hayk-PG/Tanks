using System;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [Serializable]
    private struct LevelMap
    {
        [SerializeField]
        internal Texture2D _map;
        internal Color _pixelColor;
    }

    [SerializeField]
    private LevelMap _levelMap;

    /// <summary>
    /// 0: 36FF00 T
    /// 1: 38D90D TL
    /// 2: 2DB908 TR
    /// 3: 289F08 L
    /// 4: 1F8204 R
    /// 5: 175905 RL
    /// 6: 103C04 RTL
    /// 7: 0C3002 LS
    /// 8: 081F02 RS
    /// 9: B9714E M
    /// </summary>
    [SerializeField]
    private ColorToPrefab[] _colorMapping;
    private TilesData _tilesData;
    private ChangeTiles _changeTiles;
    private Vector3 _position;



    private void Awake()
    {
        _tilesData = Get<TilesData>.From(gameObject);
        _changeTiles = Get<ChangeTiles>.From(gameObject);
    }

    private void Start()
    {
        GenerateLevel();
    }

    private void GenerateLevel()
    {
        for (int x = 0; x < _levelMap._map.width; x++)
        {
            for (int y = 0; y < _levelMap._map.height; y++)
            {
                GenerateTile(x, y);
            }
        }

        _changeTiles.UpdateTiles();
    }

    private void GenerateTile(int x, int y)
    {
        _levelMap._pixelColor = _levelMap._map.GetPixel(x, y);

        if (_levelMap._pixelColor.a == 0)
        {
            return;
        }

        float _x = ((float)x / 2) - 5;
        float _y = ((float)y / 2) - 5;

        _position = new Vector3(_x, _y, 0);

        foreach (var colorTile in _colorMapping)
        {
            if (colorTile._color == _levelMap._pixelColor)
            {
                GameObject tile = Instantiate(colorTile._prefab, _position, Quaternion.identity, transform);
                _tilesData.TilesDict.Add(_position, tile);
            }
        }
    }
}
