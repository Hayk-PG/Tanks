using Photon.Pun;
using UnityEngine;

public class PlatformsSerializeView : BaseGameManagerSerializeView
{
    [SerializeField] private Platform _horizPlatform;
    [SerializeField] private Platform _vertPlatform;

    protected override void Write(PhotonStream stream)
    {
        stream.SendNext(_horizPlatform.SynchedPosition);
        stream.SendNext(_vertPlatform.SynchedPosition);
    }

    protected override void Read(PhotonStream stream, PhotonMessageInfo info)
    {
        _horizPlatform.SynchedPosition = (Vector3)stream.ReceiveNext();
        _vertPlatform.SynchedPosition = (Vector3)stream.ReceiveNext();
    }
}
