public class Tab_AITanks : BaseTab_Tanks<ConfirmTankBtn>
{
    private void OnEnable()
    {
        _object.onConfirmTankOffline += OpenTab;
    }

    private void OnDisable()
    {
        _object.onConfirmTankOffline -= OpenTab;
    }

    public override void OpenTab()
    {
        base.OpenTab();
    }
}