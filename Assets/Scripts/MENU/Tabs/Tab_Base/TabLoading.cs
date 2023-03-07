using System.Collections;
using UnityEngine;

public class TabLoading : TabTransition, IReset
{
    [SerializeField] 
    private GameObject _loading;

    private bool _isCoroutineRunning;




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

        StartCloseDelay();
    }

    public void Close()
    {
        SetActivity(false);

        StopCloseDelay();
    }

    private void SetActivity(bool isActive)
    {
        GlobalFunctions.CanvasGroupActivity(_canvasGroup, isActive);

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

            print(seconds);

            yield return null;
        }

        if (seconds >= 5)
        {
            Close();

            Btn[] btns = transform.parent.GetComponentsInChildren<Btn>();

            foreach (var btn in btns)
                btn.Deselect();
        }
    }

    public void SetDefault() => Close();
}
