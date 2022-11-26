using UnityEngine;

public abstract class BaseTanksList : MonoBehaviour 
{
    [SerializeField] protected TanksHorizontalGroup[] _horizontalGroups;

    public struct Parameters
    {
        public int _index;
        public int _horizontalGroupsLength;
        public TankProperties[] _tankProperties;
        public TankProperties[] _dataTanksList;
    }

    protected abstract int SelectedTankIndex { get; }



    protected abstract void OnEnable();

    protected abstract void OnDisable();

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
        _horizontalGroups[parameters._index].Initialize(parameters, SelectedTankIndex);
    }
}