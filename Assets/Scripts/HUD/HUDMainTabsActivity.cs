using UnityEngine;

public class HUDMainTabsActivity : MonoBehaviour
{
    [SerializeField] 
    private CanvasGroup[] _canvasGroups;

    [SerializeField] [Space]
    private GameplayAnnouncer _gameplayAnnouncer;

    [SerializeField] [Space]
    private AmmoTypeController _ammoTypeController;




    private void Start() => CanvasGroupsActivity(false);

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
