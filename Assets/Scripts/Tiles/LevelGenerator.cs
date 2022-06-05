using UnityEngine;


public class LevelGenerator : MonoBehaviour
{
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
    [SerializeField] private ColorToPrefab[] _colorMapping;
    [SerializeField] private ColorToPrefab[] _tanksSpawnPoints;
    [SerializeField] private Maps _maps;
    [SerializeField] private int _currentMapIndex;

    private TilesData _tilesData;
    private ChangeTiles _changeTiles;
    private Vector3 _position;
    private Color _pixelColor;  
    
    public int CurrentMapIndex
    {
        get => _currentMapIndex;
        set => _currentMapIndex = value;
    }
    public float MapHorizontalStartPoint { get; private set; }
    public float MapHorizontalEndPoint { get; private set; }

    private Texture2D MapTexture => _maps.All[CurrentMapIndex].Texture;
    private float MapTextureWidth => MapTexture.width;
    private float MapTextureHeight => MapTexture.height;
    private float QuarterOfMapTextureWidth => MapTextureWidth / 4;
    private float QuarterOfMapTextureHeight => MapTextureHeight / 4;



    private void Awake()
    {
        _tilesData = Get<TilesData>.From(gameObject);
        _changeTiles = Get<ChangeTiles>.From(gameObject);
    }

    private void Start()
    {
        LoopMapTexturePixels(true);      
    }

    public void LoopMapTexturePixels(bool updateTiles)
    {
        for (int x = 0; x < MapTextureWidth; x++)
        {
            for (int y = 0; y < MapTextureHeight; y++)
            {
                GetPixelColor(x, y, out bool isPixelTransparent);

                if (!isPixelTransparent)
                {
                    GetPixelCoordinates(x, y, out float _x, out float _y);
                    GetMapStartEndPoints(x, _x);
                    SetTankSpawnPositions(x, y);
                    InstantiateTiles();
                }
            }
        }

        if (updateTiles) _changeTiles.UpdateTiles();
    }

    private void GetPixelColor(int x, int y, out bool isPixelTransparent)
    {
        _pixelColor = MapTexture.GetPixel(x, y);
        isPixelTransparent = _pixelColor.a == 0;
    }

    private void GetPixelCoordinates(int x, int y, out float _x, out float _y)
    {
        _x = ((float)x / 2) - QuarterOfMapTextureWidth;
        _y = ((float)y / 2) - QuarterOfMapTextureHeight;
        _position = new Vector3(_x, _y, 0);
    }

    private void InstantiateTiles()
    {
        GlobalFunctions.Loop<ColorToPrefab>.Foreach(_colorMapping, colorTile =>
        {
            if (colorTile._color == _pixelColor)
            {
                GameObject tile = Instantiate(colorTile._prefab, _position, Quaternion.identity, transform);
                _tilesData.TilesDict.Add(_position, tile);
            }
        });
    }

    private void GetMapStartEndPoints(int x, float _x)
    {
        bool isFirstPixel = x == 0;
        bool isLastPixel = x == MapTextureWidth - 1;

        if (isFirstPixel) MapHorizontalStartPoint = _x;
        if (isLastPixel) MapHorizontalEndPoint = _x;
    }

    private void SetTankSpawnPositions(int x, int y)
    {
        Vector2 spawnPoint1 = new Vector2(x - QuarterOfMapTextureWidth, QuarterOfMapTextureHeight);
        Vector2 spawnPoint2 = new Vector2(Mathf.Abs(_tanksSpawnPoints[0]._prefab.transform.position.x), _tanksSpawnPoints[0]._prefab.transform.position.y);

        if (_pixelColor == _tanksSpawnPoints[0]._color)
            _tanksSpawnPoints[0]._prefab.transform.position = spawnPoint1;

        if (_pixelColor == _tanksSpawnPoints[1]._color)
            _tanksSpawnPoints[1]._prefab.transform.position = spawnPoint2;
    }
}
