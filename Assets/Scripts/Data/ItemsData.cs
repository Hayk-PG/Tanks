using System;
using UnityEngine;

public partial class Data : MonoBehaviour
{
    [Serializable] public struct MyItems
    {
        public Item _item;
        public int _number;
    }

    [Header("Items")]
    [SerializeField] private MyItems _gold;
    [SerializeField] private MyItems _bulletsBox;
    [SerializeField] private MyItems _box;
    [SerializeField] private MyItems _woodenBox;
    [SerializeField] private MyItems _rewardChest;

    public int MyGolds
    {
        get => _gold._number;
        set => _gold._number = value;
    }
    public int MyBulletsBox
    {
        get => _bulletsBox._number;
        set => _bulletsBox._number = value;
    }
    public int MyBoxes
    {
        get => _box._number;
        set => _box._number = value;
    }
    public int MyWoodenBoxes
    {
        get => _woodenBox._number;
        set => _woodenBox._number = value;
    }
    public int MyRewardChest
    {
        get => _rewardChest._number;
        set => _rewardChest._number = value;
    }

    public int ItemCountByItemType(Item.ItemType itemType)
    {
        return itemType == Item.ItemType.box ? MyBoxes :
               itemType == Item.ItemType.bulletsBox ? MyBulletsBox :
               itemType == Item.ItemType.gold ? MyGolds :
               itemType == Item.ItemType.rewardChest ? MyRewardChest :
               itemType == Item.ItemType.woodenBox ? MyWoodenBoxes :
               0;
    }
}
