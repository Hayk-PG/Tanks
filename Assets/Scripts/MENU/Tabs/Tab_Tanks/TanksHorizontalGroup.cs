using UnityEngine;
using System.Linq;


public class TanksHorizontalGroup : MonoBehaviour
{
    [SerializeField] private Btn_Tank[] _btnTanks;


    public void Initialize(TankProperties[] horizontalGroupTanksList, TankProperties[] dataTanksList)
    {
        for (int i = 0; i < horizontalGroupTanksList.Length; i++)
        {
            DefineBtnTankPropeties(i, horizontalGroupTanksList[i], dataTanksList);
        }
    }

    private void DefineBtnTankPropeties(int index, TankProperties btnTankProperty, TankProperties[] dataTanksList)
    {
        _btnTanks[index].SetActivity(true);
        _btnTanks[index].SetPicture(btnTankProperty._iconTank);
        _btnTanks[index].SetName(btnTankProperty._tankName);
        _btnTanks[index].SetStars(btnTankProperty._starsCount);
        _btnTanks[index].SetRelatedTankIndex((dataTanksList.ToList().IndexOf(btnTankProperty)));
        _btnTanks[index].SetLevel(btnTankProperty._availableInLevel);
        _btnTanks[index].AutoSelect();
    }
}
