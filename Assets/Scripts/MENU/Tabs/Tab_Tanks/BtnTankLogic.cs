public class BtnTankLogic : BaseBtnTankLogic<Tab_StartGame>
{
    protected override void SaveTankIndex(int relatedTankIndex)
    {
        Data.Manager.SetData(new Data.NewData { SelectedTankIndex = relatedTankIndex });
    }
}
