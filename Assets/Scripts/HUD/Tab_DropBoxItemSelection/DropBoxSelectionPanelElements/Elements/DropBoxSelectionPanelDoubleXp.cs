using UnityEngine;
using System;

public class DropBoxSelectionPanelDoubleXp : BaseDropBoxSelectionPanelElement
{
    [SerializeField] [Space]
    private int _multiplier;

    public event Action<int> onDoubleXp;

    protected override void Use()
    {
        onDoubleXp?.Invoke(_multiplier);
    }
}
