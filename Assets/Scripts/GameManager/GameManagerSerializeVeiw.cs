using Photon.Pun;
using UnityEngine;

public class GameManagerSerializeVeiw : MonoBehaviourPun,IPunObservable
{
    private TurnController _turnController;
    private WindSystemController _windSystemController;
    private GameManagerBulletSerializer _gameManagerBulletSerializer;

    private void Awake()
    {
        _turnController = Get<TurnController>.From(gameObject);
        _windSystemController = Get<WindSystemController>.From(gameObject);
        _gameManagerBulletSerializer = Get<GameManagerBulletSerializer>.From(gameObject);
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
                stream.SendNext(_gameManagerBulletSerializer.BulletController.RigidBody.rotation);
            }
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
                _gameManagerBulletSerializer.BulletController.RigidBody.rotation = (Quaternion)stream.ReceiveNext();
            }
        }
    }    
}
