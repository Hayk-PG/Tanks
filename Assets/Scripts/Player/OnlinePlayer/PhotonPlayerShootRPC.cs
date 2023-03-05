using Photon.Pun;

public class PhotonPlayerShootRPC : PhotonPlayerBaseRPC, IShoot
{
    public void LaunchRocket(int id)
    {
        _photonPlayerController.PhotonView.RPC("LaunchRocketRPC", RpcTarget.AllViaServer, id);
    }

    [PunRPC]
    private void LaunchRocketRPC(int id)
    {
        _photonPlayerTankController?._shootController.LaunchRocket(id);
    }

    public void Shoot(float force)
    {
        _photonPlayerController.PhotonView.RPC("ShootRPC", RpcTarget.AllViaServer, force);
    }

    [PunRPC]
    private void ShootRPC(float force)
    {
        _photonPlayerTankController?._shootController.ShootBullet(force);
    }
}
