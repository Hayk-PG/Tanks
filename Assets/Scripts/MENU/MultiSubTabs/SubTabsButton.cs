using UnityEngine;
using System;

[RequireComponent(typeof(Btn))]

public class SubTabsButton : MonoBehaviour
{
    private Btn _btn;

    public event Action onSelect;
    public event Action onDeselect;


    private void Awake()
    {
        _btn = Get<Btn>.From(gameObject);
    }

    private void OnEnable()
    {
        _btn.onSelect += delegate { onSelect?.Invoke(); };
        _btn.onDeselect += delegate { onDeselect?.Invoke(); };
    }

    private void OnDisable()
    {
        _btn.onSelect -= delegate { onSelect?.Invoke(); };
        _btn.onDeselect -= delegate { onDeselect?.Invoke(); };
    }

    public void Select()
    {
        _btn.Select();
    }

    public void Deselect()
    {
        _btn.Deselect();
    }
}
