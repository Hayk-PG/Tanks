using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public abstract class BaseToggleCheck : MonoBehaviour
{
    protected Toggle _toggle;

    public bool IsOn
    {
        get => _toggle.isOn;
        set => _toggle.isOn = value;
    }

    public event Action<bool> onToggleValueChange;



    protected virtual void Awake()
    {
        _toggle = Get<Toggle>.FromChild(gameObject);
    }

    protected void Update()
    {
        _toggle.onValueChanged.RemoveAllListeners();
        _toggle.onValueChanged.AddListener(GetValue);
    }

    protected virtual void GetValue(bool isOn) => onToggleValueChange?.Invoke(isOn);
}
