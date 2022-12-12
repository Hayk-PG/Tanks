using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LobbyItem : MonoBehaviour
{
    public enum ItemType { Coin, Master, Strength}
    private CanvasGroup _canvasGroup;
    [SerializeField] private ItemType _itemType;
    [SerializeField] private TMP_Text _txt;
    [SerializeField] private Image _img;
    [SerializeField] private Sprite _sprtCoin, _sprtStrength, _sprtMaster;
    [SerializeField] private int _amount;

    public string Type { get; private set; }
    public int Quantity { get; private set; }


    private void Awake()
    {
        _canvasGroup = Get<CanvasGroup>.From(gameObject);
        HideItemOnNullAmount();
        SetItemTypeAndAmount();
    }

    private void HideItemOnNullAmount()
    {
        if (_amount == 0)
            GlobalFunctions.CanvasGroupActivity(_canvasGroup, false);
    }

    private void SetItemTypeAndAmount()
    {
        Type = _itemType == ItemType.Coin ? Keys.ItemCoins : _itemType == ItemType.Master ? Keys.ItemMaster : Keys.ItemStrength;       
        Quantity = _amount;
        _img.sprite = _itemType == ItemType.Coin ? _sprtCoin : _itemType == ItemType.Master ? _sprtMaster : _sprtStrength;
        _txt.text = Mathf.Abs(_amount).ToString();
    }
}
