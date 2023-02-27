using UnityEngine;
using System;

public class DropBoxSelectionPanelHealth : BaseDropBoxSelectionPanelElement
{
    [SerializeField] [Space]
    private int _health;

    public event Action<int> onUpdateHealth;


    protected override void Use()
    {
        onUpdateHealth?.Invoke(_health);
    }
}
