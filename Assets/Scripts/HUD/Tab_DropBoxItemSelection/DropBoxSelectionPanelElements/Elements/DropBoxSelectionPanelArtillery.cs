using System;
using UnityEngine;


public class DropBoxSelectionPanelArtillery : BaseDropBoxSelectionPanelElement
{
    [SerializeField] [Space]
    private float _shellsSpreadValue;

    public event Action<float, int, int> onArtillery;

    protected override void Use()
    {
        onArtillery?.Invoke(_shellsSpreadValue, -_price, _quantity);

        CanUse = false;
    }
}
