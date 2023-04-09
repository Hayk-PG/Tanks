using UnityEngine;

public class TabTransition : MonoBehaviour
{
    protected CanvasGroup _canvasGroup;
    protected Tab_Base _tabBase;


    protected virtual void Awake()
    {
        _canvasGroup = Get<CanvasGroup>.From(gameObject);

        _tabBase = Get<Tab_Base>.From(gameObject);
    }

    protected virtual void OnEnable()
    {
        if (_tabBase == null)
            return;

        _tabBase.onGoBack += Open;
        _tabBase.onGoForward += Open;
        _tabBase.onTabOpen += Open;
    }

    protected virtual void OnDisable()
    {
        if (_tabBase == null)
            return;

        _tabBase.onGoBack -= Open;
        _tabBase.onGoForward -= Open;
        _tabBase.onTabOpen -= Open;
    }

    private void Open()
    {
        GlobalFunctions.CanvasGroupActivity(_canvasGroup, true);

        Invoke("Close", 0.5f);
    }

    private void Close() => GlobalFunctions.CanvasGroupActivity(_canvasGroup, false);
}
