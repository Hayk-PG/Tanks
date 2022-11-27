using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LobbyItem : MonoBehaviour
{
    public enum ItemType { Coin, Strength, Master, None }
    public ItemType _itemType;

    [SerializeField] private TMP_Text _txt;
    [SerializeField] private Image _img;

    [SerializeField] private Sprite _sprtCoin, _sprtStrength, _sprtMaster;

    [SerializeField] private int _amount;



    private void Awake()
    {
        SetItemType(_itemType);
    }

    public void SetItemType(ItemType itemType)
    {
        switch (itemType)
        {
            case ItemType.Coin: _img.sprite = _sprtCoin; break;
            case ItemType.Strength: _img.sprite = _sprtStrength; break;
            case ItemType.Master: _img.sprite = _sprtMaster; break;
            case ItemType.None: gameObject.SetActive(false); break;
        }
    }
}
