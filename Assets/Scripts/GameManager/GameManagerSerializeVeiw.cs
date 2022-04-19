using Photon.Pun;

public class GameManagerSerializeVeiw : MonoBehaviourPun,IPunObservable
{
    private TurnController _turnController;


    private void Awake()
    {
        _turnController = Get<TurnController>.From(gameObject);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            //stream.SendNext(_turnController._turnState);
            //stream.SendNext(_turnController._previousTurnState);
        }
        else
        {
            //_turnController._turnState = (TurnState)stream.ReceiveNext();
            //_turnController._previousTurnState = (TurnState)stream.ReceiveNext();
        }
    }    
}
