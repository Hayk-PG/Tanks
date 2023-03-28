using UnityEngine;
using System;

public class DropBoxSelectionPanelHealth : BaseDropBoxSelectionPanelElement
{
    [SerializeField] [Space]
    private int _health;


    protected override void Use() => DropBoxSelectionHandler.RaiseEvent(DropBoxItemType.Health, new object[] { NegativePrice, _health });
}
