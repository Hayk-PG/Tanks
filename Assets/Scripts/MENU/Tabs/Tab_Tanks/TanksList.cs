public class TanksList : BaseTanksList
{
    protected override int SelectedTankIndex => Data.Manager.SelectedTankIndex;

    protected override void OnEnable()
    {
        MenuTabs.Tab_Tanks.onTabOpen += delegate { SetTanksList(Data.Manager.AvailableTanks); };
    }

    protected override void OnDisable()
    {
        MenuTabs.Tab_Tanks.onTabOpen -= delegate { SetTanksList(Data.Manager.AvailableTanks); };
    }   
}
