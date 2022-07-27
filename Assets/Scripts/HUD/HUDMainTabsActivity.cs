using UnityEngine;

public class HUDMainTabsActivity : MonoBehaviour
{
    [SerializeField] private CanvasGroup[] _canvasGroups;
    private GameplayAnnouncer _gameplayAnnouncer;


    private void Awake()
    {
        _gameplayAnnouncer = FindObjectOfType<GameplayAnnouncer>();
        CanvasGroupsActivity(false);
    }

    private void OnEnable()
    {
        _gameplayAnnouncer.OnGameStartAnnouncement += delegate { CanvasGroupsActivity(true); };
    }

    private void OnDisable()
    {
        _gameplayAnnouncer.OnGameStartAnnouncement -= delegate { CanvasGroupsActivity(false); };
    }

    private void CanvasGroupsActivity(bool isActive)
    {
        GlobalFunctions.Loop<CanvasGroup>.Foreach(_canvasGroups, canvasgroup =>
        {
            GlobalFunctions.CanvasGroupActivity(canvasgroup, isActive);
        });
    }
}
