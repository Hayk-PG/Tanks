using System.Collections;
using UnityEngine;

public class TabLoading : TabTransition, IReset
{
    public enum LoadingState { ConnectingToMasterServer}

    [SerializeField] [Space]
    private GameObject _loading;

    [SerializeField] [Space]
    private bool _dontDeselectBtns;

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

    public void Open(float waitTime = 0)
    {
        SetActivity(true);

        StartCloseDelay(waitTime);
    }

    public void Close()
    {
        SetActivity(false);

        StopCloseDelay();

        DeselectBtns();
    }

    private void SetActivity(bool isActive)
    {
        GlobalFunctions.CanvasGroupActivity(_canvasGroup, isActive);

        _loading.SetActive(isActive);
    }

    private void DeselectBtns()
    {
        if (_dontDeselectBtns)
            return;

        if (_baseTabBtns == null)
            _baseTabBtns = transform.parent.GetComponentsInChildren<Btn>();

        foreach (var btn in _baseTabBtns)
            btn.Deselect();
    }

    private void StartCloseDelay(float waitTime = 0)
    {
        StopCloseDelay();

        StartCoroutine(CloseScreenAfterDelay(waitTime));
    }

    private void StopCloseDelay() => _isCoroutineRunning = false;

    private IEnumerator CloseScreenAfterDelay(float waitTime = 0)
    {
        float elapsedTime = 0;

        _isCoroutineRunning = true;

        while (_isCoroutineRunning && elapsedTime < waitTime)
        {
            elapsedTime += Time.deltaTime;

            GlobalFunctions.DebugLog($"CloseScreenAfterDelay: {elapsedTime:0}");

            if (elapsedTime >= waitTime)
            {
                Close();

                yield break;
            }

            yield return null;
        }
    }

    public void SetDefault() => Close();
}
