using System;
using UnityEngine;

public class Options : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    private IReset[] _iResets;

    public event Action<bool> onOptionsActivity;


    private void Awake()
    {
        _iResets = GetComponentsInChildren<IReset>();
    }

    public void Activity(bool isActive)
    {
        GlobalFunctions.CanvasGroupActivity(_canvasGroup, isActive);
        GlobalFunctions.Loop<IReset>.Foreach(_iResets, iReset => { iReset?.SetDefault(); });
        onOptionsActivity?.Invoke(isActive);
    }
}
