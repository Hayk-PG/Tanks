using UnityEngine;
using System;


public enum DropBoxItemType { Ability, XpDoubleBoost, XpTripleBoost, XpUpgrade, HpBoost, Shield, Ammo, Rocket, Bomber, Artillery, C4 }

public class DropBoxSelectionHandler : MonoBehaviour
{
    public static event Action<DropBoxItemType, object[]> onItemSelect;

    public static  void RaiseEvent(DropBoxItemType dropBoxItemType, object[] data)
    {
        onItemSelect?.Invoke(dropBoxItemType, data);
    }
}
