using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Bar_UserItems : MonoBehaviour, IReset
{
    private CanvasGroup _canvasGroups;

    [SerializeField]
    private Item[] _items;

    [SerializeField] [Space]
    private Image[] _imgItems;

    [SerializeField] [Space]
    private TMP_Text[] _txtItems;



    private void Awake()
    {
        _canvasGroups = Get<CanvasGroup>.From(gameObject);

        SetItemsIcons();
    }

    private void SetItemsIcons()
    {
        for (int i = 0; i < _items.Length; i++)
            _imgItems[i].sprite = _items[i].Icon;
    }

    private void PrintUserItems()
    {
        _txtItems[0].text = Data.Manager.Coins.ToString();
        _txtItems[1].text = Data.Manager.Masters.ToString();
        _txtItems[2].text = Data.Manager.Strengths.ToString();
    }

    public void SetDefault()
    {
        GlobalFunctions.CanvasGroupActivity(_canvasGroups, !MyPhotonNetwork.IsOfflineMode);

        if (MyPhotonNetwork.IsOfflineMode)
            return;

        PrintUserItems();
    }
}
