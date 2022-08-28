using UnityEngine;

public class Tab_Settings : MonoBehaviour
{
    [SerializeField] private TopBarSettingsButtons _topBarSettingsButton;
    private CanvasGroup _canvasGroup;


    private void Awake()
    {
        _canvasGroup = Get<CanvasGroup>.From(gameObject);
    }

    private void OnEnable()
    {
        if (_topBarSettingsButton != null) _topBarSettingsButton.OnSettingsButtonClicked += OpenTab;
    }

    private void OnDisable()
    {
        if (_topBarSettingsButton != null) _topBarSettingsButton.OnSettingsButtonClicked -= OpenTab;
    }

    private void OpenTab()
    {
        GlobalFunctions.CanvasGroupActivity(_canvasGroup, true);
        UISoundController.PlaySound(1, 2);
    }

    public void CloseTab()
    {
        GlobalFunctions.CanvasGroupActivity(_canvasGroup, false);
        UISoundController.PlaySound(1, 3);
    }
}
