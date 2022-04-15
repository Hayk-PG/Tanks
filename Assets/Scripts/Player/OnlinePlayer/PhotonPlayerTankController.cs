
public class PhotonPlayerTankController : BasePlayerTankController<BasePlayer>
{
    protected override void GetTankControl()
    {
        if (photonView.IsMine)
        {
            OwnTank?.GetTankControl(_playerController);
        }
    }
}
