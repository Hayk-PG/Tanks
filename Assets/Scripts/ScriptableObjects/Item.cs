using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable objects/Item")]
public class Item : ScriptableObject
{
    public enum ItemType { Coin, Master, Strength}

    [SerializeField] private ItemType _itemType;
    [SerializeField] private Sprite _icon;
  
    public ItemType Type
    {
        get => _itemType;
        private set => _itemType = value;
    }
    public Sprite Icon => _icon;
    public string Name
    {
        get
        {
            return _itemType == ItemType.Coin ? Keys.ItemCoins :
                   _itemType == ItemType.Master ? Keys.ItemMaster :
                   Keys.ItemStrength;
        }
    }
}
