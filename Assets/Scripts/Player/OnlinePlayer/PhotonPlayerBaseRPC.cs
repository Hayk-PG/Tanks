using Photon.Pun;

public class PhotonPlayerBaseRPC : MonoBehaviourPun
{
    protected PhotonPlayerController _photonPlayerController;
    protected PhotonPlayerTankController _photonPlayerTankController;

    protected virtual void Awake()
    {
        _photonPlayerController = Get<PhotonPlayerController>.From(gameObject);
        _photonPlayerTankController = Get<PhotonPlayerTankController>.From(gameObject);
    }
}
