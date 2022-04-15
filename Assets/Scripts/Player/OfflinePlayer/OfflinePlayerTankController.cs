
public class OfflinePlayerTankController : BasePlayerTankController<BasePlayer>
{
    protected override void GetTankControl()
    {
        OwnTank?.GetTankControl(_playerController);
    }
}
