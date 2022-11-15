
public class TanksList : BaseTanksList<Tab_StartGame>
{
    protected override TankProperties[] DataTanks()
    {
        return Data.Manager.AvailableTanks;
    }
}
