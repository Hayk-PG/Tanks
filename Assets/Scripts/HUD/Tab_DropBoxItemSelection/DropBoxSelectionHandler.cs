using UnityEngine;
using System;


public enum DropBoxItemType { Xp2, Xp3, Ammo, Health, Rocket, Bomber, Artillery }

public class DropBoxSelectionHandler : MonoBehaviour
{
    public static event Action<DropBoxItemType, object[]> onItemSelect;

    public static  void RaiseEvent(DropBoxItemType dropBoxItemType, object[] data)
    {
        onItemSelect?.Invoke(dropBoxItemType, data);
    }
}
