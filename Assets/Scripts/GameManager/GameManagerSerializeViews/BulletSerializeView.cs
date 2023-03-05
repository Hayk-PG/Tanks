using Photon.Pun;
using UnityEngine;

public class BulletSerializeView : BaseGameManagerSerializeView
{
    protected override void Read(PhotonStream stream, PhotonMessageInfo info)
    {
        if (GameSceneObjectsReferences.GameManagerBulletSerializer.BaseBulletController == null)
            return;

        GameSceneObjectsReferences.GameManagerBulletSerializer.BaseBulletController.SynchedPosition = (Vector3)stream.ReceiveNext();
        GameSceneObjectsReferences.GameManagerBulletSerializer.BaseBulletController.SynchedRotation = (Quaternion)stream.ReceiveNext();
    }

    protected override void Write(PhotonStream stream)
    {
        if (GameSceneObjectsReferences.GameManagerBulletSerializer.BaseBulletController == null)
            return;

        stream.SendNext(GameSceneObjectsReferences.GameManagerBulletSerializer.BaseBulletController.SynchedPosition);
        stream.SendNext(GameSceneObjectsReferences.GameManagerBulletSerializer.BaseBulletController.SynchedRotation);
    }
}
