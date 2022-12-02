using UnityEngine;

public class TabTransition : MonoBehaviour
{
    private CanvasGroup _canvasGroup;
    private Tab_Base _tabBase;


    private void Awake()
    {
        _canvasGroup = Get<CanvasGroup>.From(gameObject);
        _tabBase = Get<Tab_Base>.From(gameObject);
        transform.SetAsLastSibling();
    }

    private void OnEnable()
    {
        _tabBase.onGoBack += Open;
        _tabBase.onGoForward += Open;
        _tabBase.onTabOpen += Open;
    }

    private void OnDisable()
    {
        _tabBase.onGoBack -= Open;
        _tabBase.onGoForward -= Open;
        _tabBase.onTabOpen -= Open;
    }

    private void Open()
    {
        GlobalFunctions.CanvasGroupActivity(_canvasGroup, true);
        Invoke("Close", 0.5f);
    }

    private void Close()
    {
        GlobalFunctions.CanvasGroupActivity(_canvasGroup, false);
    }
}
