using UnityEngine;

public class SubTab : MonoBehaviour
{
    [SerializeField] private SubTabsButton _subTabsButton;
    private CanvasGroup _canvasGroup;



    private void Awake()
    {
        _canvasGroup = Get<CanvasGroup>.From(gameObject);
    }

    private void OnEnable()
    {
        if (_subTabsButton != null)
        {
            _subTabsButton.onSelect += Open;
            _subTabsButton.onDeselect += Close;
        }
    }

    private void OnDisable()
    {
        if (_subTabsButton != null)
        {
            _subTabsButton.onSelect -= Open;
            _subTabsButton.onDeselect -= Close;
        }
    }

    private void SubTabActivity(bool isActive)
    {
        GlobalFunctions.CanvasGroupActivity(_canvasGroup, isActive);
    }

    public void Open()
    {
        SubTabActivity(true);
        GlobalFunctions.DebugLog("Open");
    }

    public void Close()
    {
        SubTabActivity(false);
        GlobalFunctions.DebugLog("Close");
    }
}
