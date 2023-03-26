using System.Collections;
using UnityEngine;

public class GameSceneObjectsReferences : MonoBehaviour
{
    public static GameSceneObjectsReferences Instance { get; private set; }

    [Header("GameManager")]

    [SerializeField] [Space]
    private GameManager _gameManager;

    [SerializeField]
    private BaseEndGame[] _baseEndGames;

    [SerializeField] 
    private TurnController _turnController;

    [SerializeField] 
    private WindSystemController _windSystemController;

    [SerializeField]
    private AirSupport _airSupport;

    [SerializeField]
    private ArtillerySupport _artillerySupport;

    [SerializeField] 
    private GameManagerBulletSerializer _gameManagerBulletSerializer;

    [SerializeField] 
    private PhotonNetworkAALauncher _photonNetworkAALauncher;

    [SerializeField]
    private WoodBoxSerializer _woodBoxSerializer;

    [SerializeField]
    private WoodBoxSerializeView _woodBoxSerializeView;

    [SerializeField]
    private PhotonNetworkWeatherManager _photonNetworkWeatherManager;

    [SerializeField]
    private PhotonNetworkMineController _photonNetworkMineController;


    [Header("Tab_Ammo")]

    [SerializeField] [Space]
    private Tab_WoodboxContent _tabWoodboxContent;

    [SerializeField]
    private AmmoTabCustomization _ammoTabCustomization;

    [SerializeField]
    private AmmoTabButtonNotification _ammoTabNotification;


    [Header("TileGenerator")]

    [SerializeField] [Space]
    private TilesData _tilesData;

    [SerializeField] 
    private ChangeTiles _changeTiles;

    [SerializeField] 
    private LevelCreator _levelCreator;

    [SerializeField]
    private MapPoints _mapPoints;


    [Header("Camera")]

    [SerializeField] [Space]
    private MainCameraController _mainCameraController;

    [SerializeField] 
    private WeatherManager _weatherManager;


    [Header("DropBoxSelectionPanel")]

    [SerializeField] [Space]
    private Tab_DropBoxItemSelection _tabDropBoxItemSelection;

    [SerializeField]
    private DropBoxSelectionPanelBomber[] _dropBoxSelectionPanelBombers;

    [SerializeField]
    private DropBoxSelectionPanelRocket[] _dropBoxSelectionPanelRockets;

    [SerializeField]
    private DropBoxSelectionPanelArtillery[] _dropBoxSelectionPanelArtillery;

    [SerializeField]
    private DropBoxSelectionPanelHealth[] _dropBoxSelectionPanelHealth;

    [SerializeField]
    private DropBoxSelectionPanelAmmo _dropBoxSelectionPanelAmmo;

    [SerializeField]
    private DropBoxSelectionPanelDoubleXp _dropBoxSelectionPanelDoubleXp;

    [SerializeField]
    private DropBoxSelectionPanelShield _dropBoxSelectionPanelShield;

    [SerializeField]
    private DropBoxSelectionPanelScores _dropBoxSelectionPanelScores;

    [SerializeField]
    private DropBoxSelectionPanelC4 _dropBoxSelectionPanelC4;

    [SerializeField]
    private DropBoxItemSelectionPanelOwner _dropBoxItemSelectionPanelOwner;


    [Header("HUD")]

    [SerializeField] [Space]
    private HudTabsHandler _hudTabsHandler;

    [SerializeField]
    private BaseRemoteControlTarget _baseRemoteControlTarget;

    [SerializeField]
    private Tab_RocketController _tabRocketController;

    [SerializeField]
    private Controllers _controllers;

    [SerializeField]
    private GameplayAnnouncer _gamePlayAnnouncer;



    //GameManager
    public static GameManager GameManager => Instance._gameManager;
    public static BaseEndGame BaseEndGame
    {
        get => Instance._baseEndGames[0].enabled ? Instance._baseEndGames[0] : Instance._baseEndGames[1];
    }
    public static TurnController TurnController => Instance._turnController;
    public static WindSystemController WindSystemController => Instance._windSystemController;
    public static AirSupport AirSupport => Instance._airSupport;
    public static ArtillerySupport ArtillerySupport => Instance._artillerySupport;
    public static GameManagerBulletSerializer GameManagerBulletSerializer => Instance._gameManagerBulletSerializer;
    public static PhotonNetworkAALauncher PhotonNetworkAALauncher => Instance._photonNetworkAALauncher;
    public static WoodBoxSerializer WoodBoxSerializer => Instance._woodBoxSerializer;
    public static WoodBoxSerializeView WoodBoxSerializeView => Instance._woodBoxSerializeView;
    public static PhotonNetworkWeatherManager PhotonNetworkWeatherManager => Instance._photonNetworkWeatherManager;
    public static PhotonNetworkMineController PhotonNetworkMineController => Instance._photonNetworkMineController;  

    //Tab_Ammo
    public static Tab_WoodboxContent Tab_WoodboxContent => Instance._tabWoodboxContent;
    public static AmmoTabCustomization AmmoTabCustomization => Instance._ammoTabCustomization;
    public static AmmoTabButtonNotification AmmoTabButtonNotification => Instance._ammoTabNotification;

    //TileGenerator
    public static TilesData TilesData => Instance._tilesData;
    public static ChangeTiles ChangeTiles => Instance._changeTiles;
    public static LevelCreator LevelCreator => Instance._levelCreator;
    public static MapPoints MapPoints => Instance._mapPoints;

    //Camera
    public static MainCameraController MainCameraController => Instance._mainCameraController;
    public static WeatherManager WeatherManager => Instance._weatherManager;

    //DropBoxSelectionPanel
    public static Tab_DropBoxItemSelection Tab_DropBoxItemSelection => Instance._tabDropBoxItemSelection;
    public static DropBoxSelectionPanelBomber[] DropBoxSelectionPanelBombers => Instance._dropBoxSelectionPanelBombers;
    public static DropBoxSelectionPanelRocket[] DropBoxSelectionPanelRockets => Instance._dropBoxSelectionPanelRockets;
    public static DropBoxSelectionPanelArtillery[] DropBoxSelectionPanelArtillery => Instance._dropBoxSelectionPanelArtillery;
    public static DropBoxSelectionPanelHealth[] DropBoxSelectionPanelHealth => Instance._dropBoxSelectionPanelHealth;
    public static DropBoxSelectionPanelAmmo DropBoxSelectionPanelAmmo => Instance._dropBoxSelectionPanelAmmo;
    public static DropBoxSelectionPanelDoubleXp DropBoxSelectionPanelDoubleXp => Instance._dropBoxSelectionPanelDoubleXp;
    public static DropBoxSelectionPanelShield DropBoxSelectionPanelShield => Instance._dropBoxSelectionPanelShield;
    public static DropBoxSelectionPanelScores DropBoxSelectionPanelScores => Instance._dropBoxSelectionPanelScores;
    public static DropBoxSelectionPanelC4 DropBoxSelectionPanelC4 => Instance._dropBoxSelectionPanelC4;
    public static DropBoxItemSelectionPanelOwner DropBoxItemSelectionPanelOwner => Instance._dropBoxItemSelectionPanelOwner;

    //HUD
    public static HudTabsHandler HudTabsHandler => Instance._hudTabsHandler;
    public static BaseRemoteControlTarget BaseRemoteControlTarget => Instance._baseRemoteControlTarget;
    public static Tab_RocketController TabRocketController => Instance._tabRocketController;
    public static Controllers Controllers => Instance._controllers;
    public static GameplayAnnouncer GameplayAnnouncer => Instance._gamePlayAnnouncer;



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

