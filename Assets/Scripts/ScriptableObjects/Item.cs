using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable objects/Item")]
public class Item : ScriptableObject
{
    public enum ItemType { gold, bulletsBox, box, woodenBox, rewardChest}

    [SerializeField] private ItemType _itemType;
    [SerializeField] private Sprite _icon;
  
    public ItemType Type
    {
        get => _itemType;
        private set => _itemType = value;
    }
    public Sprite Icon => _icon;
}
