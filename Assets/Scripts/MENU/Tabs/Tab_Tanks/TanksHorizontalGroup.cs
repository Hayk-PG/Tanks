using UnityEngine;
using System.Linq;


public class TanksHorizontalGroup : MonoBehaviour
{
    [SerializeField] private Btn_Tank[] _btnTanks;

    public struct Parameters
    {
        public int _index;
        public int _horizontalGroupTanksLength;
        public TankProperties _btnTankProperty;
        public TankProperties[] _dataTanksList;
    }

    public void Initialize(int horizontalGroupsLength, TankProperties[] tankProperties, TankProperties[] dataTanksList)
    {
        for (int i = 0; i < tankProperties.Length; i++)
        {
            Parameters parameters = new Parameters
            {
                _index = i,
                _horizontalGroupTanksLength = horizontalGroupsLength,
                _btnTankProperty = tankProperties[i],
                _dataTanksList = dataTanksList
            };

            DefineBtnTankPropeties(parameters);
        }
    }

    private void DefineBtnTankPropeties(Parameters parameters)
    {
        _btnTanks[parameters._index].SetActivity(true);
        _btnTanks[parameters._index].SetPicture(parameters._btnTankProperty._iconTank);
        _btnTanks[parameters._index].SetName(parameters._btnTankProperty._tankName);
        _btnTanks[parameters._index].SetStars(parameters._btnTankProperty._starsCount);
        _btnTanks[parameters._index].SetRelatedTankIndex((parameters._dataTanksList.ToList().IndexOf(parameters._btnTankProperty)));
        _btnTanks[parameters._index].SetLevel(parameters._btnTankProperty._availableInLevel);
        _btnTanks[parameters._index].AutoSelect(Data.Manager.SelectedTankIndex, parameters._horizontalGroupTanksLength);
    }
}
