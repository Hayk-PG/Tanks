using System.Collections;
using UnityEngine;

public class Tab_Modify : MonoBehaviour
{
    [SerializeField] private Btn _btnClose;
    [SerializeField] private BuildModeSwitcher _buildModeSwitcher;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private HUDMainTabsActivity _hudMainTabsActivity;
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private TurnController _turnController;
    private IReset[] _iResets;
    
    private bool _isTabOpen;

    public Transform LocalPlayerTransform { get; private set; }
    public PlayerTurn LocalPlayerTurn { get; private set; }
    public ScoreController LocalPlayerScoreController { get; private set; }



    private void Awake() => _iResets = GetComponentsInChildren<IReset>();

    private void OnEnable()
    {
        _buildModeSwitcher.onSelect += OpenTab;
        _gameManager.OnGameStarted += OnGameStarted;
        _turnController.OnTurnChanged += delegate { CloseTab(); };
        _btnClose.onSelect += CloseTab;
    }

    private void OnDisable()
    {
        _buildModeSwitcher.onSelect -= OpenTab;
        _gameManager.OnGameStarted -= OnGameStarted;
        _turnController.OnTurnChanged -= delegate { CloseTab(); };
        _btnClose.onSelect -= CloseTab;
    }

    private void OnGameStarted()
    {
        LocalPlayerTransform = GlobalFunctions.ObjectsOfType<TankController>.Find(tankController => tankController.BasePlayer != null)?.transform;

        if (LocalPlayerTransform != null)
        {
            LocalPlayerTurn = Get<PlayerTurn>.From(LocalPlayerTransform.gameObject);
            LocalPlayerScoreController = Get<ScoreController>.From(LocalPlayerTransform.gameObject);
        }
    }

    private void OpenTab() => StartCoroutine(OpenTabCoroutine());

    private IEnumerator OpenTabCoroutine()
    {
        yield return new WaitForSeconds(0.01f);

        if ((bool)LocalPlayerTurn?.IsMyTurn)
        {
            _hudMainTabsActivity.CanvasGroupsActivity(false);
            GlobalFunctions.Loop<IReset>.Foreach(_iResets, iReset => { iReset.SetDefault(); });
            GlobalFunctions.CanvasGroupActivity(_canvasGroup, true);
            _isTabOpen = true;
        }
    }

    private void CloseTab()
    {
        if (_isTabOpen)
        {
            _hudMainTabsActivity.CanvasGroupsActivity(true);
            GlobalFunctions.CanvasGroupActivity(_canvasGroup, false);
            GlobalFunctions.Loop<TileModifyGUI>.Foreach(FindObjectsOfType<TileModifyGUI>(), tileModifyGUI => { tileModifyGUI.DisableGUI(); });
            _isTabOpen = false;
        }
    }
}
