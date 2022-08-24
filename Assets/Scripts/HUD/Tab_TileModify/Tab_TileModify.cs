using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tab_TileModify : MonoBehaviour
{   
    internal class LocalPlayer
    {
        internal PlayerTurn _localPlayerTurn;
        internal ScoreController _localPlayerScoreController;
        internal Transform _localPlayerTransform;
    }
    private CanvasGroup _canvasGroup;
    private PropsTabCustomization _propsTabCustomization;
    private HUDMainTabsActivity _hudMainTabsActivity;
    private TilesData _tilesData;
    private GameManager _gameManager;
    private TurnController _turnController;
    private LocalPlayer _localPlayer;   
    private List<GameObject> foundTiles;
    [SerializeField] private TMP_Text _scoreText;
    private int _priceForOneTile = 0;

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
        get => _localPlayer._localPlayerScoreController.Score >= _priceForOneTile;
    }
    public bool IsTab_TileModifyOpen { get; private set; }



    private void Awake()
    {
        _canvasGroup = Get<CanvasGroup>.From(gameObject);
        _propsTabCustomization = FindObjectOfType<PropsTabCustomization>();
        _hudMainTabsActivity = FindObjectOfType<HUDMainTabsActivity>();
        _tilesData = FindObjectOfType<TilesData>();
        _gameManager = FindObjectOfType<GameManager>();
        _turnController = FindObjectOfType<TurnController>();
    }

    private void OnEnable()
    {
        _gameManager.OnGameStarted += OnGameStarted;
        _propsTabCustomization.OnModifyGround += OnModifyGround;
        _turnController.OnTurnChanged += OnTurnChanged;
    }

    private void OnDisable()
    {
        _gameManager.OnGameStarted -= OnGameStarted;
        _propsTabCustomization.OnModifyGround -= OnModifyGround;
        _turnController.OnTurnChanged -= OnTurnChanged;
    }

    private void OnGameStarted()
    {
        InitializeLocalPlayer();
    }

    private void OnModifyGround()
    {
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
                        Get<TileModifyGUI>.FromChild(tile).EnableGUI();
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
            int newScore = _localPlayer._localPlayerScoreController.Score - _priceForOneTile;
            ScoreText = newScore.ToString();
            _localPlayer._localPlayerScoreController.GetScore(-_priceForOneTile, null);
        }
    }
}
