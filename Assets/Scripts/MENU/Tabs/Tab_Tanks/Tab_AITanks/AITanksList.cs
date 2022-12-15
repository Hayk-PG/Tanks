public class AITanksList : BaseTanksList
{
    protected override int SelectedTankIndex => Data.Manager.SelectedAITankIndex;


    protected override void OnEnable()
    {
        MenuTabs.Tab_HomeOffline.onOpenTabAITanks += delegate { SetTanksList(Data.Manager.AvailableAITanks); };
    }

    protected override void OnDisable()
    {
        MenuTabs.Tab_HomeOffline.onOpenTabAITanks -= delegate { SetTanksList(Data.Manager.AvailableAITanks); };
    }
}
