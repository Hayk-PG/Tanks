using Photon.Pun;
using UnityEngine;

public class PlatformsSerializeView : BaseGameManagerSerializeView
{
    [SerializeField] private Platform _horizPlatform, _vertPlatform;



    protected override void Write(PhotonStream stream)
    {
        Vector3 horizPosition = _horizPlatform.SynchedPosition ?? Vector3.zero;
        Vector3 vertPosition = _vertPlatform.SynchedPosition ?? Vector3.zero;

        stream.SendNext(horizPosition);
        stream.SendNext(vertPosition);
    }

    protected override void Read(PhotonStream stream, PhotonMessageInfo info)
    {
        Vector3 horizPosition = (Vector3)stream.ReceiveNext();
        Vector3 vertPosition = (Vector3)stream.ReceiveNext();

        if (_horizPlatform != null)
            _horizPlatform.SynchedPosition = horizPosition;

        if (_vertPlatform != null)
            _vertPlatform.SynchedPosition = vertPosition;
    }
}
