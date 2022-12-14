using Photon.Pun;


public class GlobalActivityTimerSerializeView : BaseGameManagerSerializeView
{
    protected override void Write(PhotonStream stream)
    {
        stream.SendNext(_globalActivtyTimer._playersActiveShieldsTimer[0]);
        stream.SendNext(_globalActivtyTimer._playersActiveShieldsTimer[1]);
    }

    protected override void Read(PhotonStream stream, PhotonMessageInfo info)
    {
        _globalActivtyTimer._playersActiveShieldsTimer[0] = (int)stream.ReceiveNext();
        _globalActivtyTimer._playersActiveShieldsTimer[1] = (int)stream.ReceiveNext();
    }
}