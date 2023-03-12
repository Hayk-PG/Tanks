using UnityEngine;
using System.Linq;


public class TanksHorizontalGroup : MonoBehaviour
{
    [SerializeField]
    private Btn_Tank[] _btnTanks;

    public struct Parameters
    {
        public int _index;
        public int _horizontalGroupTanksLength;

        public TankProperties _btnTankProperty;

        public TankProperties[] _dataTanksList;
    }

    public void Initialize(BaseTanksList.Parameters baseTanksListParameters, int selectedTankIndex)
    {
        for (int i = 0; i < baseTanksListParameters._tankProperties.Length; i++)
        {
            Parameters parameters = new Parameters
            {
                _index = i,

                _horizontalGroupTanksLength = baseTanksListParameters._horizontalGroupsLength,

                _btnTankProperty = baseTanksListParameters._tankProperties[i],

                _dataTanksList = baseTanksListParameters._dataTanksList
            };

            DefineBtnTankPropeties(parameters, selectedTankIndex);
        }
    }

    private void DefineBtnTankPropeties(Parameters parameters, int selectedTankIndex)
    {
        _btnTanks[parameters._index].SetActivity(true);
        _btnTanks[parameters._index].SetTankProprties(parameters._btnTankProperty);
        _btnTanks[parameters._index].SetPicture(parameters._btnTankProperty._iconTank);
        _btnTanks[parameters._index].SetName(parameters._btnTankProperty._tankName);
        _btnTanks[parameters._index].SetStars(parameters._btnTankProperty._starsCount);
        _btnTanks[parameters._index].SetRelatedTankIndex((parameters._dataTanksList.ToList().IndexOf(parameters._btnTankProperty)));
        _btnTanks[parameters._index].SetLevel(parameters._btnTankProperty._availableInLevel);
        _btnTanks[parameters._index].SetLockState(false);
        _btnTanks[parameters._index].AutoSelect(selectedTankIndex, parameters._horizontalGroupTanksLength);
    }
}
