using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tab_TileModify : MonoBehaviour
{
    public enum TileModifyType { NewTile, ArmoredCube, ArmoredTile }
    internal class LocalPlayer
    {
        internal PlayerTurn _localPlayerTurn;
        internal ScoreController _localPlayerScoreController;
        internal Transform _localPlayerTransform;
    }
    [SerializeField] 
    private TMP_Text _scoreText;
    [SerializeField] 
    private TMP_Text _priceText;

    private TileModifyType _tileModifyType;
    private CanvasGroup _canvasGroup;
    private PropsTabCustomization _propsTabCustomization;
    private HUDMainTabsActivity _hudMainTabsActivity;
    private TilesData _tilesData;
    private GameManager _gameManager;
    private TurnController _turnController;
    private LocalPlayer _localPlayer;   
    private List<GameObject> foundTiles;
    
    private int Price
    {
        get => int.Parse(_priceText.text);
        set => _priceText.text = value.ToString();
    }
    private string ScoreText
    {
        get => _scoreText.text;
        set => _scoreText.text = value;
    }
    private bool IsLocalInitialized
    {
        get => _localPlayer != null;
    }
    public bool CanModifyTiles
    {
        get => _localPlayer._localPlayerScoreController.Score >= Price;
    }
    public bool IsTab_TileModifyOpen { get; private set; }
    public int GroundModifyPrice { get; private set; } = 250;
    public int ArmoredCubePrice { get; private set; } = 1000;
    public int ArmoredTilePrice { get; private set; } = 1000;



    private void Awake()
    {
        _canvasGroup = Get<CanvasGroup>.From(gameObject);
        _propsTabCustomization = FindObjectOfType<PropsTabCustomization>();
        _hudMainTabsActivity = FindObjectOfType<HUDMainTabsActivity>();
        _tilesData = FindObjectOfType<TilesData>();
        _gameManager = FindObjectOfType<GameManager>();
        _turnController = FindObjectOfType<TurnController>();
        _priceText.text = 0.ToString();
    }

    private void OnEnable()
    {
        _gameManager.OnGameStarted += OnGameStarted;
        _propsTabCustomization.OnModifyGround += OnModifyGround;
        _propsTabCustomization.OnInstantiateMetalCube += OnInstantiateMetalCube;
        _propsTabCustomization.OnChangeToMetalGround += OnChangeToMetalGround;
        _turnController.OnTurnChanged += OnTurnChanged;
    }

    private void OnDisable()
    {
        _gameManager.OnGameStarted -= OnGameStarted;
        _propsTabCustomization.OnModifyGround -= OnModifyGround;
        _propsTabCustomization.OnInstantiateMetalCube -= OnInstantiateMetalCube;
        _propsTabCustomization.OnChangeToMetalGround -= OnChangeToMetalGround;
        _turnController.OnTurnChanged -= OnTurnChanged;
    }

    private void SetTileModifyType(TileModifyType tileModifyType)
    {
        _tileModifyType = tileModifyType;
    }

    private void SetPrice(int price)
    {
        Price = price;
    }

    private void OnGameStarted()
    {
        InitializeLocalPlayer();
    }

    private void OnModifyGround()
    {
        SetPrice(GroundModifyPrice);
        SetTileModifyType(TileModifyType.NewTile);
        Invoke("FindTilesAroundPlayer", 0.1f);
    }

    private void OnInstantiateMetalCube()
    {
        SetPrice(ArmoredCubePrice);
        SetTileModifyType(TileModifyType.ArmoredCube);
        Invoke("FindTilesAroundPlayer", 0.1f);
    }

    private void OnChangeToMetalGround()
    {
        SetPrice(ArmoredTilePrice);
        SetTileModifyType(TileModifyType.ArmoredTile);
        Invoke("FindTilesAroundPlayer", 0.1f);
    }

    private void OnTurnChanged(TurnState turnState)
    {
        OnClickToClose();
    }

    private void InitializeLocalPlayer()
    {
        Transform localPlayerTransform = GlobalFunctions.ObjectsOfType<TankController>.Find(tank => tank.BasePlayer != null).transform;

        if (localPlayerTransform != null)
        {
            _localPlayer = new LocalPlayer
            {
                _localPlayerTransform = localPlayerTransform,
                _localPlayerTurn = Get<PlayerTurn>.From(localPlayerTransform.gameObject),
                _localPlayerScoreController = Get<ScoreController>.From(localPlayerTransform.gameObject)
            };
        }
    }

    public void FindTilesAroundPlayer()
    {
        if (IsLocalInitialized)
        {
            if (_localPlayer._localPlayerTurn.IsMyTurn)
            {
                _hudMainTabsActivity.CanvasGroupsActivity(false);
                GlobalFunctions.CanvasGroupActivity(_canvasGroup, true);
                ScoreText = _localPlayer._localPlayerScoreController.Score.ToString();
                foundTiles = new List<GameObject>();
                IsTab_TileModifyOpen = true;

                foreach (var tile in _tilesData.TilesDict)
                {
                    bool haveLeftTilesBeenFound = tile.Key.x <= _localPlayer._localPlayerTransform.position.x - _tilesData.Size && tile.Key.x >= _localPlayer._localPlayerTransform.position.x - (_tilesData.Size * 6);
                    bool haveRIghtTilesBeenFound = tile.Key.x >= _localPlayer._localPlayerTransform.position.x + _tilesData.Size && tile.Key.x <= _localPlayer._localPlayerTransform.position.x + (_tilesData.Size * 6);

                    if (haveLeftTilesBeenFound && tile.Value != null && Get<TileModifyGUI>.FromChild(tile.Value) != null)
                        foundTiles.Add(tile.Value);

                    if (haveRIghtTilesBeenFound && tile.Value != null && Get<TileModifyGUI>.FromChild(tile.Value) != null)
                        foundTiles.Add(tile.Value);
                }

                foreach (var tile in foundTiles)
                {
                    if (!Get<Tile>.From(tile).IsProtected)
                    {
                        Get<TileModifyGUI>.FromChild(tile).EnableGUI(_tileModifyType);
                    }
                }
            }
        }
    }

    public void OnClickToClose()
    {
        foreach (var gui in FindObjectsOfType<TileModifyGUI>())
        {
            gui.DisableGUI();
        }
      
        GlobalFunctions.CanvasGroupActivity(_canvasGroup, false);

        if (IsTab_TileModifyOpen)
            _hudMainTabsActivity.CanvasGroupsActivity(true);

        IsTab_TileModifyOpen = false;
    }

    public void SubtractScore()
    {
        if (IsLocalInitialized)
        {
            int newScore = _localPlayer._localPlayerScoreController.Score - Price;
            ScoreText = newScore.ToString();
            _localPlayer._localPlayerScoreController.GetScore(-Price, null);
        }
    }
}
