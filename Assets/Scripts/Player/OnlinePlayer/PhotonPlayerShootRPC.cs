using Photon.Pun;

public class PhotonPlayerShootRPC : PhotonPlayerBaseRPC, IShoot
{
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
