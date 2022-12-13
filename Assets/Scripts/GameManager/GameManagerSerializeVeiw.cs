using Photon.Pun;
using UnityEngine;

public class GameManagerSerializeVeiw : MonoBehaviourPun,IPunObservable
{
    private TurnController _turnController;
    private WindSystemController _windSystemController;
    private GameManagerBulletSerializer _gameManagerBulletSerializer;
    private TurnTimer _turnTimer;
    private GlobalActivityTimer _globalActivtyTimer;
    private InstantiatePickables _instantiatePickables;
    private WoodenBoxSerializer _woodenBoxSerializer;
    private PlatformSerializer _platformSerializer;
    private GlobalExplosiveBarrels _globalExplosiveBarrels;

    private void Awake()
    {
        _turnController = Get<TurnController>.From(gameObject);
        _windSystemController = Get<WindSystemController>.From(gameObject);
        _gameManagerBulletSerializer = Get<GameManagerBulletSerializer>.From(gameObject);
        _turnTimer = Get<TurnTimer>.From(gameObject);
        _globalActivtyTimer = Get<GlobalActivityTimer>.From(gameObject);
        _instantiatePickables = Get<InstantiatePickables>.From(gameObject);
        _woodenBoxSerializer = Get<WoodenBoxSerializer>.From(gameObject);
        _platformSerializer = Get<PlatformSerializer>.From(gameObject);
        _globalExplosiveBarrels = Get<GlobalExplosiveBarrels>.From(gameObject);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting && info.Sender.IsMasterClient)
        {
            stream.SendNext(_turnController._turnState);
            stream.SendNext(_turnController._previousTurnState);

            stream.SendNext(_windSystemController.CurrentWindForce);

            if(_gameManagerBulletSerializer.BulletController != null)
            {
                stream.SendNext(_gameManagerBulletSerializer.BulletController.RigidBody.position);
                stream.SendNext(_gameManagerBulletSerializer.BulletController.RigidBody.velocity);
                stream.SendNext(_gameManagerBulletSerializer.BulletController.RigidBody.rotation);
            }

            foreach (var bullet in _gameManagerBulletSerializer.MultipleBulletsController)
            {
                if (bullet != null)
                {
                    stream.SendNext(bullet.RigidBody.position);
                    stream.SendNext(bullet.RigidBody.velocity);
                    stream.SendNext(bullet.RigidBody.rotation);
                }
            }

            stream.SendNext(_turnTimer.Seconds);
            stream.SendNext(_turnTimer.IsTurnChanged);

            stream.SendNext(_globalActivtyTimer._playersActiveShieldsTimer[0]);
            stream.SendNext(_globalActivtyTimer._playersActiveShieldsTimer[1]);

            if (_woodenBoxSerializer.ParachuteWithWoodBoxController != null)
            {
                stream.SendNext(_woodenBoxSerializer.ParachuteWithWoodBoxController.RandomDestroyTime);
                stream.SendNext(_woodenBoxSerializer.ParachuteWithWoodBoxController.RigidBody.position);
                stream.SendNext(_woodenBoxSerializer.ParachuteWithWoodBoxController.RigidBody.velocity);
                stream.SendNext(_woodenBoxSerializer.ParachuteWithWoodBoxController.RigidBody.rotation);
            }

            if(_woodenBoxSerializer.WoodBox != null)
            {
                stream.SendNext(_woodenBoxSerializer.WoodBox.ContentIndex);
                stream.SendNext(_woodenBoxSerializer.WoodBox.WeaponIndex);
            }

            if (_platformSerializer.RigidbodyPlatformHor != null)
            {
                stream.SendNext(_platformSerializer.RigidbodyPlatformHor.velocity);
                stream.SendNext(_platformSerializer.RigidbodyPlatformHor.position);
            }

            if (_platformSerializer.RigidbodyPlatformVert != null)
            {
                stream.SendNext(_platformSerializer.RigidbodyPlatformVert.velocity);
                stream.SendNext(_platformSerializer.RigidbodyPlatformVert.position);
            }

            for (int i = 0; i < _globalExplosiveBarrels.BarrelRigidBody.Length; i++)
            {
                stream.SendNext(_globalExplosiveBarrels.BarrelRigidBody[i]?.position);
            }
        }
        if(stream.IsReading && !info.Sender.IsMasterClient)
        {
            _turnController._turnState = (TurnState)stream?.ReceiveNext();
            _turnController._previousTurnState = (TurnState)stream?.ReceiveNext();

            _windSystemController.CurrentWindForce = (int)stream?.ReceiveNext();

            if (_gameManagerBulletSerializer.BulletController != null)
            {
                _gameManagerBulletSerializer.BulletController.RigidBody.position = (Vector3)stream?.ReceiveNext();
                _gameManagerBulletSerializer.BulletController.RigidBody.velocity = (Vector3)stream?.ReceiveNext();
                _gameManagerBulletSerializer.BulletController.RigidBody.rotation = (Quaternion)stream?.ReceiveNext();

                float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));
                _gameManagerBulletSerializer.BulletController.RigidBody.position += (_gameManagerBulletSerializer.BulletController.RigidBody.velocity * lag);
            }

            foreach (var bullet in _gameManagerBulletSerializer.MultipleBulletsController)
            {
                if (bullet != null)
                {
                    bullet.RigidBody.position = (Vector3)stream?.ReceiveNext();
                    bullet.RigidBody.velocity = (Vector3)stream?.ReceiveNext();
                    bullet.RigidBody.rotation = (Quaternion)stream?.ReceiveNext();

                    float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));
                    bullet.RigidBody.position += (bullet.RigidBody.velocity * lag);
                }
            }

            _turnTimer.Seconds = (int)stream?.ReceiveNext();
            _turnTimer.IsTurnChanged = (bool)stream?.ReceiveNext();

            _globalActivtyTimer._playersActiveShieldsTimer[0] = (int)stream?.ReceiveNext();
            _globalActivtyTimer._playersActiveShieldsTimer[1] = (int)stream?.ReceiveNext();

            if (_woodenBoxSerializer.ParachuteWithWoodBoxController != null)
            {
                
                _woodenBoxSerializer.ParachuteWithWoodBoxController.RandomDestroyTime = (int)stream?.ReceiveNext();
                _woodenBoxSerializer.ParachuteWithWoodBoxController.RigidBody.position = (Vector3)stream?.ReceiveNext();
                _woodenBoxSerializer.ParachuteWithWoodBoxController.RigidBody.velocity = (Vector3)stream?.ReceiveNext();
                _woodenBoxSerializer.ParachuteWithWoodBoxController.RigidBody.rotation = (Quaternion)stream?.ReceiveNext();

                float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));
                _woodenBoxSerializer.ParachuteWithWoodBoxController.RigidBody.position += (_woodenBoxSerializer.ParachuteWithWoodBoxController.RigidBody.velocity * lag);
            }

            if (_woodenBoxSerializer.WoodBox != null)
            {
                _woodenBoxSerializer.WoodBox.ContentIndex = (int)stream?.ReceiveNext();
                _woodenBoxSerializer.WoodBox.WeaponIndex = (int)stream?.ReceiveNext();
            }

            if (_platformSerializer.RigidbodyPlatformHor != null)
            {
                _platformSerializer.RigidbodyPlatformHor.position = (Vector3)stream?.ReceiveNext();
                _platformSerializer.RigidbodyPlatformHor.velocity = (Vector3)stream?.ReceiveNext();

                float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));
                _platformSerializer.RigidbodyPlatformHor.position += (_platformSerializer.RigidbodyPlatformHor.velocity * lag);
            }

            if (_platformSerializer.RigidbodyPlatformVert != null)
            {
                _platformSerializer.RigidbodyPlatformVert.position = (Vector3)stream?.ReceiveNext();
                _platformSerializer.RigidbodyPlatformVert.velocity = (Vector3)stream?.ReceiveNext();

                float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));
                _platformSerializer.RigidbodyPlatformVert.position += (_platformSerializer.RigidbodyPlatformVert.velocity * lag);
            }

            for (int i = 0; i < _globalExplosiveBarrels.BarrelRigidBody.Length; i++)
            {
                if (_globalExplosiveBarrels.BarrelRigidBody[i] != null)
                {
                    _globalExplosiveBarrels.BarrelRigidBody[i].position = (Vector3)stream?.ReceiveNext();
                }
            }
        }
    }    
}
