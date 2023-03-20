using UnityEngine;

public class HUDMainTabsActivity : MonoBehaviour, IEndGame
{
    [SerializeField] 
    private CanvasGroup[] _canvasGroups;

    [SerializeField] [Space]
    private GameplayAnnouncer _gameplayAnnouncer;

    [SerializeField] [Space]
    private AmmoTypeController _ammoTypeController;

    private bool _isLocked;



    private void Start() => CanvasGroupsActivity(false);

    private void OnEnable()
    {
        _gameplayAnnouncer.OnGameStartAnnouncement += delegate { CanvasGroupsActivity(true); };

        _ammoTypeController.OnInformAboutTabActivityToTabsCustomization += OnWeaponsTabActivity;

        GameSceneObjectsReferences.BaseRemoteControlTarget.onRemoteControlActivity += Lock;

        GameSceneObjectsReferences.TabRocketController.onActivity += isOpen => { CanvasGroupsActivity(!isOpen); };
    }

    private void OnDisable()
    {
        _gameplayAnnouncer.OnGameStartAnnouncement -= delegate { CanvasGroupsActivity(false); };

        _ammoTypeController.OnInformAboutTabActivityToTabsCustomization -= OnWeaponsTabActivity;

        GameSceneObjectsReferences.BaseRemoteControlTarget.onRemoteControlActivity -= Lock;

        GameSceneObjectsReferences.TabRocketController.onActivity -= isOpen => { CanvasGroupsActivity(!isOpen); };
    }

    private void Lock(bool isActive)
    {
        _isLocked = isActive;

        CanvasGroupsActivity(!isActive);
    }

    public void CanvasGroupsActivity(bool isActive)
    {
        if (_isLocked)
        {
            GlobalFunctions.Loop<CanvasGroup>.Foreach(_canvasGroups, canvasgroup =>
            {
                GlobalFunctions.CanvasGroupActivity(canvasgroup, false);
            });

            return;
        }

        GlobalFunctions.Loop<CanvasGroup>.Foreach(_canvasGroups, canvasgroup =>
        {
            GlobalFunctions.CanvasGroupActivity(canvasgroup, isActive);
        });
    }

    private void OnWeaponsTabActivity(bool isOpen)
    {
        if (_isLocked)
        {
            GlobalFunctions.CanvasGroupActivity(_canvasGroups[1], false);

            return;
        }

        GlobalFunctions.CanvasGroupActivity(_canvasGroups[1], !isOpen);
    }

    public void OnGameEnd(object[] data = null)
    {
        Lock(true);
    }
}
