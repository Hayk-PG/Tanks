using UnityEngine;
using System;

public class DropBoxSelectionHandler : MonoBehaviour
{
    public enum DropBoxItemType { XpMultiplier }

    public static event Action<DropBoxItemType, object[]> onItemSelect;

    public static  void RaiseEvent(DropBoxItemType dropBoxItemType, object[] data)
    {
        onItemSelect?.Invoke(dropBoxItemType, data);
    }
}
