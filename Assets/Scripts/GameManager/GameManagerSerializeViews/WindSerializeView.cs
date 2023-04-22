using Photon.Pun;

public class WindSerializeView : BaseGameManagerSerializeView
{
    protected override void Write(PhotonStream stream) => stream.SendNext(_windSystemController.CurrentWindForce);

    protected override void Read(PhotonStream stream, PhotonMessageInfo info) => _windSystemController.CurrentWindForce = (int)stream.ReceiveNext();
}
