using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Collections;

public class Tab_TileModify : MonoBehaviour
{
    public enum TileModifyType { NewTile, ArmoredCube, ArmoredTile }
    internal class LocalPlayer
    {
        internal PlayerTurn _turn;
        internal ScoreController _score;
        internal Transform _transform;
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
        get => _localPlayer._score.Score >= Price;
    }
    public bool IsTab_TileModifyOpen { get; private set; }

    public class Prices
    {
        public string Name { get; set; }
        public int Price { get; set; }
    }

    public Prices[] NewPrices = new Prices[3]
    {
        new Prices{Name = Names.ModifyGround, Price = 0},
        new Prices{Name = Names.MetalCube, Price = 1000},
        new Prices{Name = Names.MetalGround, Price = 1000}
    };


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
        SetPrice(NewPrices[0].Price);
        SetTileModifyType(TileModifyType.NewTile);
        StartCoroutine(StartFindTilesAroundPlayer());
    }

    private void OnInstantiateMetalCube()
    {
        SetPrice(NewPrices[1].Price);
        SetTileModifyType(TileModifyType.ArmoredCube);
        StartCoroutine(StartFindTilesAroundPlayer());
    }

    private void OnChangeToMetalGround()
    {
        SetPrice(NewPrices[2].Price);
        SetTileModifyType(TileModifyType.ArmoredTile);
        StartCoroutine(StartFindTilesAroundPlayer());
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
                _transform = localPlayerTransform,
                _turn = Get<PlayerTurn>.From(localPlayerTransform.gameObject),
                _score = Get<ScoreController>.From(localPlayerTransform.gameObject)
            };
        }
    }

    private void StoreFoundTiles(bool canStore, GameObject tile)
    {
        if (canStore && tile != null && Get<TileModifyGUI>.FromChild(tile) != null)
        {
            foundTiles.Add(tile);
        }
    }

    public void FindTilesAroundPlayer()
    {
        if (IsLocalInitialized)
        {
            if (_localPlayer._turn.IsMyTurn)
            {
                _hudMainTabsActivity.CanvasGroupsActivity(false);
                GlobalFunctions.CanvasGroupActivity(_canvasGroup, true);
                ScoreText = _localPlayer._score.Score.ToString();
                foundTiles = new List<GameObject>();
                IsTab_TileModifyOpen = true;

                foreach (var tile in _tilesData.TilesDict)
                {
                    if (_tileModifyType != TileModifyType.ArmoredTile)
                    {
                        bool haveLeftTilesBeenFound = tile.Key.x <= _localPlayer._transform.position.x - _tilesData.Size && tile.Key.x >= _localPlayer._transform.position.x - (_tilesData.Size * 6);
                        bool haveRIghtTilesBeenFound = tile.Key.x >= _localPlayer._transform.position.x + _tilesData.Size && tile.Key.x <= _localPlayer._transform.position.x + (_tilesData.Size * 6);
                        StoreFoundTiles(haveLeftTilesBeenFound, tile.Value);
                        StoreFoundTiles(haveRIghtTilesBeenFound, tile.Value);
                    }
                    else
                    {
                        bool foundNearTiles = tile.Key.x >= _localPlayer._transform.position.x - (_tilesData.Size * 6) && tile.Key.x <= _localPlayer._transform.position.x + (_tilesData.Size * 6);
                        StoreFoundTiles(foundNearTiles, tile.Value);
                    }
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

    private IEnumerator StartFindTilesAroundPlayer()
    {
        yield return new WaitForSeconds(0.01f);
        FindTilesAroundPlayer();
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
            int newScore = _localPlayer._score.Score - Price;
            ScoreText = newScore.ToString();
            _localPlayer._score.GetScore(-Price, null);
        }
    }
}
