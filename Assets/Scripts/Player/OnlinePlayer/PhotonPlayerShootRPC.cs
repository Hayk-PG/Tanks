using Photon.Pun;
using UnityEngine;

public class PhotonPlayerShootRPC : PhotonPlayerBaseRPC
{
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
