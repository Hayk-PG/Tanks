using System;
using UnityEngine;

public class SubTab : MonoBehaviour
{
    [SerializeField] private SubTabsButton _subTabsButton;
    private CanvasGroup _canvasGroup;

    public event Action<bool> _onActivity;


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
        _onActivity?.Invoke(isActive);
    }

    public void Open()
    {
        SubTabActivity(true);      
    }

    public void Close()
    {
        SubTabActivity(false);
    }
}
