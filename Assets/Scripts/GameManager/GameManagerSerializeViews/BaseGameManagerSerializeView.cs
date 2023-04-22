using Photon.Pun;

public abstract class BaseGameManagerSerializeView : MonoBehaviourPun, IPunObservable
{
    protected TurnController _turnController;
    protected WindSystemController _windSystemController;
    protected GameManagerBulletSerializer _gameManagerBulletSerializer;
    protected TurnTimer _turnTimer;
    protected GlobalActivityTimer _globalActivtyTimer;
    protected InstantiatePickables _instantiatePickables;
    protected WoodBoxSerializer _woodenBoxSerializer;




    protected virtual void Awake()
    {
        _turnController = Get<TurnController>.From(gameObject);
        _windSystemController = Get<WindSystemController>.From(gameObject);
        _gameManagerBulletSerializer = Get<GameManagerBulletSerializer>.From(gameObject);
        _turnTimer = Get<TurnTimer>.From(gameObject);
        _globalActivtyTimer = Get<GlobalActivityTimer>.From(gameObject);
        _instantiatePickables = Get<InstantiatePickables>.From(gameObject);
        _woodenBoxSerializer = Get<WoodBoxSerializer>.From(gameObject);
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting && info.Sender.IsMasterClient)
            Write(stream);
        if (stream.IsReading)
            Read(stream, info);
    }

    protected abstract void Write(PhotonStream stream);
    protected abstract void Read(PhotonStream stream, PhotonMessageInfo info);
    protected virtual float Lag(PhotonMessageInfo info)
    {
        return UnityEngine.Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));
    }
}
