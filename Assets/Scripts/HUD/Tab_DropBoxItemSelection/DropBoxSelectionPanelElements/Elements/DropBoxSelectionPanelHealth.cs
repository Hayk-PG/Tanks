using UnityEngine;
using System;

public class DropBoxSelectionPanelHealth : BaseDropBoxSelectionPanelElement
{
    [SerializeField] [Space]
    private int _health;

    public event Action<int, int> onUpdateHealth;


    protected override void Use()
    {
        onUpdateHealth?.Invoke(-_price, _health);
    }
}
