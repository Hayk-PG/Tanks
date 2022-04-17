using Photon.Pun;

public class PhotonPlayerShootRPC : MonoBehaviourPun
{
    private PhotonPlayerController _photonPlayerController;
    private PhotonPlayerTankController _photonPlayerTankController;


    private void Awake()
    {
        _photonPlayerController = Get<PhotonPlayerController>.From(gameObject);
        _photonPlayerTankController = Get<PhotonPlayerTankController>.From(gameObject);
    }

    public void CallShootRPC(float force)
    {
        _photonPlayerController.PhotonView.RPC("ShootRPC", RpcTarget.AllViaServer, force);
    }

    [PunRPC]
    private void ShootRPC(float force)
    {
        _photonPlayerTankController?._shootController.ShootBullet(force);
    }
}
