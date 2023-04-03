using UnityEngine;

public class TanksList : MonoBehaviour
{
    protected Btn_Tank[] _btnTanks;




    protected virtual void Awake() => GetAllBtnTanks();

    protected virtual int SelectedTankIndex => Data.Manager.SelectedTankIndex;

    protected virtual void OnEnable() => MenuTabs.Tab_Tanks.onTabOpen += SetTankList;

    protected virtual void OnDisable() => MenuTabs.Tab_Tanks.onTabOpen -= SetTankList;

    protected virtual void GetAllBtnTanks() => _btnTanks = GetComponentsInChildren<Btn_Tank>(true);

    protected virtual void SetTankList()
    {
        print($"Selected tank's index: {Data.Manager.SelectedTankIndex}");

        for (int i = 0; i < Data.Manager.AvailableTanks.Length; i++)
        {
            _btnTanks[i].SetActivity(true);
            _btnTanks[i].SetTankProprties(Data.Manager.AvailableTanks[i]);
            _btnTanks[i].SetPicture(Data.Manager.AvailableTanks[i].assetReferenceIcon);
            _btnTanks[i].SetName(Data.Manager.AvailableTanks[i]._tankName);
            _btnTanks[i].SetRelatedTankIndex(Data.Manager.AvailableTanks[i]._tankIndex);
            _btnTanks[i].SetStars(Data.Manager.AvailableTanks[i]._starsCount);
            _btnTanks[i].SetLevel(Data.Manager.AvailableTanks[i]._availableInLevel);
            _btnTanks[i].SetLockState(false);
            _btnTanks[i].AutoSelect(Data.Manager.SelectedTankIndex);
        }
    }
}
