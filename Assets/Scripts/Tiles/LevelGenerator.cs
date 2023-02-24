using System;
using UnityEngine;


public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private Maps _maps;
    [SerializeField] private int _currentMapIndex;
   
    public int CurrentMapIndex
    {
        get => _currentMapIndex;
        set => _currentMapIndex = value;
    }
    private float MapTextureWidth => MapTexture.width;
    private float MapTextureHeight => MapTexture.height;
    private float QuarterOfMapTextureWidth => MapTextureWidth / 4;
    private float QuarterOfMapTextureHeight => MapTextureHeight / 4;
    private Texture2D MapTexture => _maps.All[CurrentMapIndex].Texture;

    public static ChangeTiles ChangeTiles { get; private set; }
    public static TilesData TilesData { get; private set; }


    public event Action<LevelGeneratorData> onMapTexturePixels;



    private void Awake()
    {
        ChangeTiles = Get<ChangeTiles>.From(gameObject);
        TilesData = Get<TilesData>.From(gameObject);
    }

    private void Start()
    {
        GetMapIndex();
        LoopMapTexturePixels(true);      
    }

    private void GetMapIndex()
    {
        if (MyPhotonNetwork.IsOfflineMode)
            CurrentMapIndex = Data.Manager.MapIndex;
        else
            CurrentMapIndex = MyPhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(Keys.MapIndex) ? (int)MyPhotonNetwork.CurrentRoom.CustomProperties[Keys.MapIndex] : 0;
    }

    public void LoopMapTexturePixels(bool updateTiles)
    {
        LevelGeneratorData levelGeneratorData = new LevelGeneratorData();

        for (int x = 0; x < MapTextureWidth; x++)
        {
            for (int y = 0; y < MapTextureHeight; y++)
            {
                ShareLevelGeneratorData(levelGeneratorData, x, y);
            }
        }

        ChangeTiles.UpdateTiles(null);
    }

    private void ShareLevelGeneratorData(LevelGeneratorData levelGeneratorData, int x, int y)
    {
        levelGeneratorData.MapTexturePixelColor = MapTexture.GetPixel(x, y);
        levelGeneratorData.MepTextureDimension = new Vector2(x, y);
        levelGeneratorData.MapTexturePixelsCoordinate = new Vector3(((float)x / 2) - QuarterOfMapTextureWidth, ((float)y / 2) - QuarterOfMapTextureHeight, 0);
        levelGeneratorData.MapTextureWidth = MapTextureWidth;
        levelGeneratorData.MapTextureHeight = MapTextureHeight;

        onMapTexturePixels?.Invoke(levelGeneratorData);
    }
}
