using Photon.Pun;

public class TimerSerializeView : BaseGameManagerSerializeView
{
    protected override void Write(PhotonStream stream)
    {
        stream.SendNext(_turnTimer.Seconds);
        stream.SendNext(_turnTimer.IsTurnChanged);
    }

    protected override void Read(PhotonStream stream, PhotonMessageInfo info)
    {
        _turnTimer.Seconds = (int)stream.ReceiveNext();
        _turnTimer.IsTurnChanged = (bool)stream.ReceiveNext();
    }
}
