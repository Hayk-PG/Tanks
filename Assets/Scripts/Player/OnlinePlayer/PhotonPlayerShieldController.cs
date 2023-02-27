using UnityEngine;
using Photon.Pun;

public class PhotonPlayerShieldController : OfflinePlayerShieldController
{
    [SerializeField] [Space]
    private PhotonPlayerController _photonPlayerController;



    protected override void OnActivateShield()
    {
        if (_playerTankController?.OwnTank.BasePlayer == null)
            return;

        _photonPlayerController.PhotonView.RPC("ActivateShieldRPC", RpcTarget.AllViaServer, ShieldIndex());
    }

    [PunRPC]
    private void ActivateShieldRPC(int index)
    {
        _playerTankController._playerShields.ActivateShields(index);
    }
}
