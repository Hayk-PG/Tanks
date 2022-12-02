using System;
using UnityEngine;

public class Options : MonoBehaviour
{
    private CanvasGroup _canvasGroup;
    private IReset[] _iResets;

    public event Action<bool> onOptionsActivity;


    private void Awake()
    {
        _canvasGroup = Get<CanvasGroup>.From(gameObject);
        _iResets = GetComponentsInChildren<IReset>();
    }

    public void Activity(bool isActive)
    {
        GlobalFunctions.CanvasGroupActivity(_canvasGroup, isActive);
        GlobalFunctions.Loop<IReset>.Foreach(_iResets, iReset => { iReset?.SetDefault(); });
        onOptionsActivity?.Invoke(isActive);
    }
}
