using System.Collections;
using UnityEngine;

public class TabLoading : TabTransition, IReset
{
    public enum LoadingState { ConnectingToMasterServer}

    [SerializeField] 
    private GameObject _loading;

    private bool _isCoroutineRunning;

    private Btn[] _baseTabBtns;




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

    public void Open() => SetActivity(true);

    public void Close() => SetActivity(false);

    private void SetActivity(bool isActive)
    {
        GlobalFunctions.CanvasGroupActivity(_canvasGroup, isActive);

        Conditions<bool>.Compare(isActive, StartCloseDelay, StopCloseDelay);

        _loading.SetActive(isActive);
    }

    private void StartCloseDelay()
    {
        StopCloseDelay();

        StartCoroutine(CloseScreenAfterDelay());
    }

    private void StopCloseDelay() => _isCoroutineRunning = false;

    private IEnumerator CloseScreenAfterDelay()
    {
        float seconds = 0;

        _isCoroutineRunning = true;

        while (_isCoroutineRunning && seconds < 5)
        {
            seconds += Time.deltaTime;

            yield return null;
        }

        if (seconds >= 5)
        {
            Close();

            if (_baseTabBtns == null)
                _baseTabBtns = transform.parent.GetComponentsInChildren<Btn>();

            foreach (var btn in _baseTabBtns)
                btn.Deselect();
        }
    }

    public void SetDefault() => Close();
}
