using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LobbyItem : MonoBehaviour
{
    public enum ItemType { Coin, Master, Strength, None}
    [SerializeField] private ItemType _itemType;
    [SerializeField] private TMP_Text _txt;
    [SerializeField] private Image _img;
    [SerializeField] private Sprite _sprtCoin, _sprtStrength, _sprtMaster;
    [SerializeField] private int _amount;

    public ItemType Type { get; private set; }
    public int Amount { get; private set; }


    private void Awake()
    {
        DeactivateItemOnNullAmount();
        GetItemTypeAndAmount();
        SetItemIcon();
    }

    private void DeactivateItemOnNullAmount()
    {
        if (_amount == 0)
            gameObject.SetActive(false);
    }

    private void GetItemTypeAndAmount()
    {
        Type = _itemType;
        Amount = _amount;
        _txt.text = _amount.ToString();
    }

    private void SetItemIcon()
    {
        switch (_itemType)
        {
            case ItemType.Coin: _img.sprite = _sprtCoin; break;
            case ItemType.Strength: _img.sprite = _sprtStrength; break;
            case ItemType.Master: _img.sprite = _sprtMaster; break;
        }
    }
}
