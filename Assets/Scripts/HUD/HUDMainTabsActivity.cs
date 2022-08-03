using UnityEngine;

public class HUDMainTabsActivity : MonoBehaviour
{
    [SerializeField] private CanvasGroup[] _canvasGroups;
    private GameplayAnnouncer _gameplayAnnouncer;
    private AmmoTypeController _ammoTypeController;


    private void Awake()
    {
        _gameplayAnnouncer = FindObjectOfType<GameplayAnnouncer>();
        _ammoTypeController = FindObjectOfType<AmmoTypeController>();
        CanvasGroupsActivity(false);
    }

    private void OnEnable()
    {
        _gameplayAnnouncer.OnGameStartAnnouncement += delegate { CanvasGroupsActivity(true); };
        _ammoTypeController.OnInformAboutTabActivityToTabsCustomization += OnWeaponsTabActivity;
    }

    private void OnDisable()
    {
        _gameplayAnnouncer.OnGameStartAnnouncement -= delegate { CanvasGroupsActivity(false); };
        _ammoTypeController.OnInformAboutTabActivityToTabsCustomization -= OnWeaponsTabActivity;
    }

    public void CanvasGroupsActivity(bool isActive)
    {
        GlobalFunctions.Loop<CanvasGroup>.Foreach(_canvasGroups, canvasgroup =>
        {
            GlobalFunctions.CanvasGroupActivity(canvasgroup, isActive);
        });
    }

    private void OnWeaponsTabActivity(bool isOpen)
    {
        GlobalFunctions.CanvasGroupActivity(_canvasGroups[1], !isOpen);
    }
}
