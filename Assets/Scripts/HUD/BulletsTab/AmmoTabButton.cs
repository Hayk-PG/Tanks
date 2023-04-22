using System;
using UnityEngine;

public class AmmoTabButton : MonoBehaviour
{
    [SerializeField]
    private Btn _btn;

    public event Action onAmmoTabActivity;



    private void OnEnable() => _btn.onSelect += OnSelect;

    private void OnDisable() => _btn.onSelect -= OnSelect;

    private void OnSelect() => onAmmoTabActivity?.Invoke();
}
