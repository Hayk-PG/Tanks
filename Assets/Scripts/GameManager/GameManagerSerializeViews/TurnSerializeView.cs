using Photon.Pun;

public class TurnSerializeView : BaseGameManagerSerializeView
{
    protected override void Write(PhotonStream stream)
    {
        stream.SendNext(_turnController._turnState);
        stream.SendNext(_turnController._previousTurnState);
    }

    protected override void Read(PhotonStream stream, PhotonMessageInfo info)
    {
        _turnController._turnState = (TurnState)stream.ReceiveNext();
        _turnController._previousTurnState = (TurnState)stream.ReceiveNext();
    }
}
