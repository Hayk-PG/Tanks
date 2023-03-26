using System.Collections;
using UnityEngine;

public class Tab_Modify : MonoBehaviour, IHudTabsObserver
{
    [SerializeField] 
    private CanvasGroup _canvasGroup;

    [SerializeField] [Space]
    private Btn _btnClose;

    [SerializeField] [Space]
    private BuildModeSwitcher _buildModeSwitcher;

    private IReset[] _iResets;
    
    private bool _isTabOpen;

    public Transform LocalPlayerTransform { get; private set; }

    public PlayerTurn LocalPlayerTurn { get; private set; }

    public ScoreController LocalPlayerScoreController { get; private set; }




    private void Awake() => _iResets = GetComponentsInChildren<IReset>();

    private void OnEnable()
    {
        GameSceneObjectsReferences.GameManager.OnGameStarted += OnGameStarted;

        GameSceneObjectsReferences.TurnController.OnTurnChanged += delegate { CloseTab(); };

        _buildModeSwitcher.onSelect += OpenTab;

        _btnClose.onSelect += CloseTab;
    }

    private void OnDisable()
    {
        GameSceneObjectsReferences.GameManager.OnGameStarted -= OnGameStarted;

        GameSceneObjectsReferences.TurnController.OnTurnChanged -= delegate { CloseTab(); };

        _buildModeSwitcher.onSelect -= OpenTab;

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
            GameSceneObjectsReferences.HudTabsHandler.RequestTabActivityPermission(this, HudTabsHandler.HudTab.TabModify, true);
    }

    private void CloseTab()
    {
        if (_isTabOpen)
            GameSceneObjectsReferences.HudTabsHandler.RequestTabActivityPermission(this, HudTabsHandler.HudTab.TabModify, false);
    }

    public void Execute(bool isActive)
    {
        if (isActive)
        {
            GlobalFunctions.Loop<IReset>.Foreach(_iResets, iReset => { iReset.SetDefault(); });
            GlobalFunctions.CanvasGroupActivity(_canvasGroup, true);

            _isTabOpen = true;
        }
        else
        {
            GlobalFunctions.CanvasGroupActivity(_canvasGroup, false);
            GlobalFunctions.Loop<TileModifyGUI>.Foreach(FindObjectsOfType<TileModifyGUI>(), tileModifyGUI => { tileModifyGUI.DisableGUI(); });

            _isTabOpen = false;
        }
    }
}
