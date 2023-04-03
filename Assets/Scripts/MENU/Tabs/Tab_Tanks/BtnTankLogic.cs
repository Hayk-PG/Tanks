public class BtnTankLogic : BaseBtnTankLogic
{
    protected override void Awake() => _tabTanks = MenuTabs.Tab_Tanks;

    protected override void SaveTankIndex(int relatedTankIndex) => Data.Manager.SetData(new Data.NewData { SelectedTankIndex = relatedTankIndex });
}
