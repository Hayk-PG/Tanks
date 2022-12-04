using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Bar_UserItems : MonoBehaviour
{
    private Tab_Base _tabBase;
    private CanvasGroup _canvasGroups;

    [SerializeField] private Item[] _items;
    [SerializeField] private Image[] _imgItems;
    [SerializeField] private TMP_Text[] _txtItems;


    private void Awake()
    {
        _tabBase = Get<Tab_Base>.From(gameObject);
        _canvasGroups = Get<CanvasGroup>.From(gameObject);

        SetItemsIcons();
    }

    private void OnEnable()
    {
        _tabBase.onTabOpen += Open;
        _tabBase.onTabClose += delegate { SetActivity(false); };
    }

    private void OnDisable()
    {
        _tabBase.onTabOpen -= Open;
        _tabBase.onTabClose -= delegate { SetActivity(false); };
    }

    private void SetItemsIcons()
    {
        for (int i = 0; i < _items.Length; i++)
        {
            _imgItems[i].sprite = _items[i].Icon;
        }
    }

    private void Open()
    {
        if (MyPhotonNetwork.IsOfflineMode)
            return;

        SetActivity(true);
        PrintUserItemsNumbers();
    }

    private void SetActivity(bool isActive) => GlobalFunctions.CanvasGroupActivity(_canvasGroups, isActive);

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
