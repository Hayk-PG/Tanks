
public class AITanksList : BaseTanksList<ConfirmTankBtn>
{
    protected override TankProperties[] DataTanks()
    {
        return Data.Manager.AvailableAITanks;
    }
}
