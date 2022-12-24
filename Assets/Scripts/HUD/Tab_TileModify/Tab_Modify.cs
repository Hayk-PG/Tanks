using System.Collections;
using UnityEngine;

public class Tab_Modify : MonoBehaviour
{
    [SerializeField] private Btn _btnClose;
    private CanvasGroup _canvasGroup;
    private IReset[] _iResets;
    private HUDMainTabsActivity _hudMainTabsActivity;
    private PropsTabCustomization _propsTabCustomization;
    private GameManager _gameManager;
    private TurnController _turnController;
    
    private bool _isTabOpen;

    public Transform LocalPlayerTransform { get; private set; }
    public PlayerTurn LocalPlayerTurn { get; private set; }
    public ScoreController LocalPlayerScoreController { get; private set; }



    private void Awake()
    {
        _canvasGroup = Get<CanvasGroup>.From(gameObject);
        _iResets = GetComponentsInChildren<IReset>();
        _hudMainTabsActivity = FindObjectOfType<HUDMainTabsActivity>();
        _propsTabCustomization = FindObjectOfType<PropsTabCustomization>();
        _gameManager = FindObjectOfType<GameManager>();
        _turnController = FindObjectOfType<TurnController>();
    }

    private void OnEnable()
    {
        _propsTabCustomization.onModify += OpenTab;
        _gameManager.OnGameStarted += OnGameStarted;
        _turnController.OnTurnChanged += delegate { CloseTab(); };
        _btnClose.onSelect += CloseTab;
    }

    private void OnDisable()
    {
        _propsTabCustomization.onModify -= OpenTab;
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
        GlobalFunctions.Loop<IReset>.Foreach(_iResets, iReset => { iReset.SetDefault(); });
        yield return new WaitForSeconds(0.01f);
        if ((bool)LocalPlayerTurn?.IsMyTurn)
        {
            _hudMainTabsActivity.CanvasGroupsActivity(false);
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
