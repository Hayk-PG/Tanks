using UnityEngine;
using TMPro;

public class Bar_UserItems : MonoBehaviour
{
    private Tab_Base _tabBase;

    [SerializeField] private TMP_Text[] _txtItems;


    private void Awake()
    {
        _tabBase = Get<Tab_Base>.From(gameObject);
    }

    private void OnEnable()
    {
        _tabBase.onTabOpen += PrintUserItemsNumbers;
    }

    private void OnDisable()
    {
        _tabBase.onTabOpen -= PrintUserItemsNumbers;
    }

    private void PrintUserItemsNumbers()
    {
        UserData.GetItems(Data.Manager.PlayfabId, items =>
        {
            for (int i = 0; i < items.Length; i++)
            {
                _txtItems[i].text = items[i].ToString();
            }
        });
    }
}
