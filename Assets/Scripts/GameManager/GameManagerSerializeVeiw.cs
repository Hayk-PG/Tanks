using Photon.Pun;
using UnityEngine;

public class GameManagerSerializeVeiw : MonoBehaviourPun,IPunObservable
{
    private TurnController _turnController;
    private WindSystemController _windSystemController;
    private GameManagerBulletSerializer _gameManagerBulletSerializer;
    private TurnTimer _turnTimer;
    private GlobalActivityTimer _globalActivtyTimer;

    private void Awake()
    {
        _turnController = Get<TurnController>.From(gameObject);
        _windSystemController = Get<WindSystemController>.From(gameObject);
        _gameManagerBulletSerializer = Get<GameManagerBulletSerializer>.From(gameObject);
        _turnTimer = Get<TurnTimer>.From(gameObject);
        _globalActivtyTimer = Get<GlobalActivityTimer>.From(gameObject);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting && info.Sender.IsMasterClient)
        {
            stream.SendNext(_turnController._turnState);
            stream.SendNext(_turnController._previousTurnState);

            stream.SendNext(_windSystemController.CurrentWindForce);
            stream.SendNext(_windSystemController.CurrentInternval);

            if(_gameManagerBulletSerializer.BulletController != null)
            {
                stream.SendNext(_gameManagerBulletSerializer.BulletController.RigidBody.position);
                stream.SendNext(_gameManagerBulletSerializer.BulletController.RigidBody.velocity);
                stream.SendNext(_gameManagerBulletSerializer.BulletController.RigidBody.rotation);
            }

            stream.SendNext(_turnTimer.Timer);
            stream.SendNext(_turnTimer.IconPlayer1Alpha);
            stream.SendNext(_turnTimer.IconPlayer2Alpha);
            stream.SendNext(_turnTimer.IsTurnChanged);

            stream.SendNext(_globalActivtyTimer._playersActiveShieldsTimer[0]);
            stream.SendNext(_globalActivtyTimer._playersActiveShieldsTimer[1]);
        }
        else
        {
            _turnController._turnState = (TurnState)stream.ReceiveNext();
            _turnController._previousTurnState = (TurnState)stream.ReceiveNext();

            _windSystemController.CurrentWindForce = (int)stream.ReceiveNext();
            _windSystemController.CurrentInternval = (int)stream.ReceiveNext();

            if (_gameManagerBulletSerializer.BulletController != null)
            {
                _gameManagerBulletSerializer.BulletController.RigidBody.position = (Vector3)stream.ReceiveNext();
                _gameManagerBulletSerializer.BulletController.RigidBody.velocity = (Vector3)stream.ReceiveNext();
                _gameManagerBulletSerializer.BulletController.RigidBody.rotation = (Quaternion)stream.ReceiveNext();

                float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));
                _gameManagerBulletSerializer.BulletController.RigidBody.position += (_gameManagerBulletSerializer.BulletController.RigidBody.velocity * lag);
            }

            _turnTimer.Timer = (int)stream.ReceiveNext();
            _turnTimer.IconPlayer1Alpha = (float)stream.ReceiveNext();
            _turnTimer.IconPlayer2Alpha = (float)stream.ReceiveNext();
            _turnTimer.IsTurnChanged = (bool)stream.ReceiveNext();

            _globalActivtyTimer._playersActiveShieldsTimer[0] = (int)stream.ReceiveNext();
            _globalActivtyTimer._playersActiveShieldsTimer[1] = (int)stream.ReceiveNext();
        }
    }    
}
