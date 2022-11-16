using UnityEngine;

public abstract class BaseTanksList<T> : MonoBehaviour where T: MonoBehaviour
{
    [SerializeField] protected TanksHorizontalGroup[] _horizontalGroups;
    protected BaseTab_Tanks<T> _tabTanks;

    public struct Parameters
    {
        public int _index;
        public int _horizontalGroupsLength;
        public TankProperties[] _tankProperties;
        public TankProperties[] _dataTanksList;
    }

    protected virtual void Awake()
    {
        _tabTanks = Get<BaseTab_Tanks<T>>.From(gameObject);
    }

    protected virtual void OnEnable()
    {
        _tabTanks.onTabOpen += delegate { SetTanksList(DataTanks()); };
    }

    protected virtual void OnDisable()
    {
        _tabTanks.onTabOpen -= delegate { SetTanksList(DataTanks()); };
    }

    protected abstract TankProperties[] DataTanks();

    protected virtual void SetTanksList(TankProperties[] dataTanksList)
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

            Parameters parameters = new Parameters
            {
                _index = i,
                _horizontalGroupsLength = horizGroupsCount,
                _tankProperties = horizontalGroupTanksList,
                _dataTanksList = dataTanksList
            };

            InitializeHorizontalGroup(parameters);
        }
    }

    protected virtual void InitializeHorizontalGroup(Parameters parameters)
    {
        _horizontalGroups[parameters._index].gameObject.SetActive(true);
        _horizontalGroups[parameters._index].Initialize(parameters._horizontalGroupsLength, parameters._tankProperties, parameters._dataTanksList);
    }
}