using UnityEngine;

public class TanksList : MonoBehaviour
{
    [SerializeField] private TanksHorizontalGroup[] _horizontalGroups;
    private Tab_Tanks _tabTanks;



    private void Awake()
    {
        _tabTanks = Get<Tab_Tanks>.From(gameObject);
    }

    private void OnEnable()
    {
        _tabTanks.OnTabOpened += delegate { SetTanksList(Data.Manager.AvailableTanks); };
    }

    private void OnDisable()
    {
        _tabTanks.OnTabOpened -= delegate { SetTanksList(Data.Manager.AvailableTanks); };
    }

    private void SetTanksList(TankProperties[] dataTanksList)
    {
        int horizGroupsCount = dataTanksList.Length % 3 == 0 ? dataTanksList.Length / 3 : dataTanksList.Length / 3 + 1;
        int row = 0;
        int length = 0;

        for (int i = 0; i < horizGroupsCount; i++)
        {
            row += 3;
            length = row - dataTanksList.Length > 0 ? 3 - (row - dataTanksList.Length) : 3;
            TankProperties[] horizontalGroupTanksList = new TankProperties[length];
            
            for (int x = row - 3, p = 0; x < row; x++, p++)
            {
                if (x < dataTanksList.Length)
                {
                    horizontalGroupTanksList[p] = dataTanksList[x];
                }
            }

            InitializeHorizontalGroup(i, horizontalGroupTanksList, dataTanksList);
        }
    }

    private void InitializeHorizontalGroup(int i, TankProperties[] horizontalGroupTankProperties, TankProperties[] collecionTankProperties)
    {
        _horizontalGroups[i].gameObject.SetActive(true);
        _horizontalGroups[i].Initialize(horizontalGroupTankProperties, collecionTankProperties);
    }
}
