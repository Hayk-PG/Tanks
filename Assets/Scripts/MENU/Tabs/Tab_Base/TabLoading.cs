using UnityEngine;

public class TabLoading : TabTransition
{
    [SerializeField] private GameObject _loading;

    protected override void OnEnable()
    {
        if (_tabBase == null)
            return;

        _tabBase.onTabClose += Close;
    }

    protected override void OnDisable()
    {
        if (_tabBase == null)
            return;

        _tabBase.onTabClose -= Close;
    }

    public void Open()
    {
        SetActivity(true);
    }

    public void Close()
    {
        SetActivity(false);
    }

    private void SetActivity(bool isActive)
    {
        GlobalFunctions.CanvasGroupActivity(_canvasGroup, isActive);
        _loading.SetActive(isActive);
    }
}
