using UnityEngine;
using System;


public class DropBoxSelectionPanelBomber : BaseDropBoxSelectionPanelElement
{
    [SerializeField]
    protected BomberType _bomberType;


    public event Action<BomberType, int, int> onCallBomber;


    protected override void Use()
    {
        onCallBomber?.Invoke(_bomberType, NegativePrice, _quantity);

        CanUse = false;
    }
}
