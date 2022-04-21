using Photon.Pun;

public class PhotonPlayerLastHealthFillUpdateRPC : PhotonPlayerBaseRPC
{
    public void CallHealthBarLastFillUpdateRPC(float value)
    {
        _photonPlayerController.PhotonView.RPC("HealthBarLastFillUpdateRPC", RpcTarget.AllViaServer, value);
    }

    [PunRPC]
    private void HealthBarLastFillUpdateRPC(float value)
    {
        _photonPlayerTankController._healthBar.LastHealthFill.OnUpdate(value);
    }
}
