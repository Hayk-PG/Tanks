using UnityEngine;

public class HUDMainTabsActivity : MonoBehaviour
{
    [SerializeField] 
    private CanvasGroup[] _canvasGroups;

    [SerializeField] [Space]
    private GameplayAnnouncer _gameplayAnnouncer;

    [SerializeField] [Space]
    private AmmoTypeController _ammoTypeController;

    [SerializeField] [Space]
    private BaseRemoteControlTarget _remoteControlTarget;

    private bool _isLocked;



    private void Start() => CanvasGroupsActivity(false);

    private void OnEnable()
    {
        _remoteControlTarget.onRemoteControlActivity += Lock;

        _gameplayAnnouncer.OnGameStartAnnouncement += delegate { CanvasGroupsActivity(true); };

        _ammoTypeController.OnInformAboutTabActivityToTabsCustomization += OnWeaponsTabActivity;      
    }

    private void OnDisable()
    {
        _remoteControlTarget.onRemoteControlActivity -= Lock;

        _gameplayAnnouncer.OnGameStartAnnouncement -= delegate { CanvasGroupsActivity(false); };

        _ammoTypeController.OnInformAboutTabActivityToTabsCustomization -= OnWeaponsTabActivity;
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
}
