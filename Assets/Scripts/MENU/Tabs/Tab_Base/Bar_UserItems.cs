using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Bar_UserItems : MonoBehaviour
{
    private Tab_Base _tabBase;

    [SerializeField] private Item[] _items;
    [SerializeField] private Image[] _imgItems;
    [SerializeField] private TMP_Text[] _txtItems;


    private void Awake()
    {
        _tabBase = Get<Tab_Base>.From(gameObject);

        SetItemsIcons();
    }

    private void OnEnable()
    {
        _tabBase.onTabOpen += PrintUserItemsNumbers;
    }

    private void OnDisable()
    {
        _tabBase.onTabOpen -= PrintUserItemsNumbers;
    }

    private void SetItemsIcons()
    {
        for (int i = 0; i < _items.Length; i++)
        {
            _imgItems[i].sprite = _items[i].Icon;
        }
    }

    private void PrintUserItemsNumbers()
    {
        User.GetItems(Data.Manager.PlayfabId, items =>
        {
            for (int i = 0; i < items.Length; i++)
            {
                _txtItems[i].text = items[i].ToString();
            }
        });
    }
}
