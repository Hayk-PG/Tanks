using System;
using UnityEngine;

public class BuildModeSwitcher : MonoBehaviour
{
    [SerializeField] private Btn _btn;

    public event Action onSelect;
         

    private void OnEnable() => _btn.onSelect += OnSelect;

    private void OnDisable() => _btn.onSelect -= OnSelect;

    private void OnSelect() => onSelect?.Invoke();
}
