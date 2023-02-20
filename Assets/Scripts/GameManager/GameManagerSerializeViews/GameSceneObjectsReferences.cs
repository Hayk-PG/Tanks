using System.Collections;
using UnityEngine;

public class GameSceneObjectsReferences : MonoBehaviour
{
    public static GameSceneObjectsReferences Instance { get; private set; }

    [Header("GameManager")]

    [SerializeField]
    private GameManager _gameManager;

    [SerializeField] 
    private TurnController _turnController;

    [SerializeField] 
    private WindSystemController _windSystemController;

    [SerializeField] 
    private GameManagerBulletSerializer _gameManagerBulletSerializer;

    [SerializeField] 
    private PhotonNetworkAALauncher _photonNetworkAALauncher;

    [SerializeField]
    private WoodBoxSerializer _woodBoxSerializer;

    [SerializeField]
    private WoodBoxSerializeView _woodBoxSerializeView;

    [Header("Tab_Ammo")]

    [SerializeField] [Space]
    private Tab_WoodboxContent _tabWoodboxContent;

    [SerializeField]
    private NewWeaponFromWoodBox _newWeaponFromWoodBox;

    [Header("TileGenerator")]

    [SerializeField] [Space]
    private TilesData _tilesData;

    [SerializeField] 
    private ChangeTiles _changeTiles;

    [SerializeField] 
    private LevelCreator _levelCreator;

    [Header("Camera")]

    [SerializeField] [Space]
    private WeatherManager _weatherManager;

    //GameManager
    public static GameManager GameManager => Instance._gameManager;
    public static TurnController TurnController => Instance._turnController;
    public static WindSystemController WindSystemController => Instance._windSystemController;
    public static GameManagerBulletSerializer GameManagerBulletSerializer => Instance._gameManagerBulletSerializer;
    public static PhotonNetworkAALauncher PhotonNetworkAALauncher => Instance._photonNetworkAALauncher;
    public static WoodBoxSerializer WoodBoxSerializer => Instance._woodBoxSerializer;
    public static WoodBoxSerializeView WoodBoxSerializeView => Instance._woodBoxSerializeView;

    //Tab_Ammo
    public static Tab_WoodboxContent Tab_WoodboxContent => Instance._tabWoodboxContent;
    public static NewWeaponFromWoodBox NewWeaponFromWoodBox => Instance._newWeaponFromWoodBox;

    //TileGenerator
    public static TilesData TilesData => Instance._tilesData;
    public static ChangeTiles ChangeTiles => Instance._changeTiles;
    public static LevelCreator LevelCreator => Instance._levelCreator;

    //Camera
    public static WeatherManager WeatherManager => Instance._weatherManager;

    //LavaSplash
    public static LavaSplash LavaSplash { get; private set; }




    private void Awake()
    {
        Instance = this;

        LavaSplash = FindObjectOfType<LavaSplash>();
    }

    private void Start()
    {
        StartCoroutine(FindLavaSplash());
    }

    private IEnumerator FindLavaSplash()
    {
        yield return new WaitUntil(() => FindObjectOfType<LavaSplash>() != null);
        LavaSplash = FindObjectOfType<LavaSplash>();
    }

}

