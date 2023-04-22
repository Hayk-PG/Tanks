using Photon.Pun;
using UnityEngine;

public class BulletSerializeView : BaseGameManagerSerializeView
{
    protected override void Write(PhotonStream stream)
    {
        Vector3 position = GameSceneObjectsReferences.GameManagerBulletSerializer.BaseBulletController?.SynchedPosition ?? Vector3.zero;

        Quaternion rotation = GameSceneObjectsReferences.GameManagerBulletSerializer.BaseBulletController?.SynchedRotation ?? Quaternion.identity;

        stream.SendNext(position);

        stream.SendNext(rotation);
    }

    protected override void Read(PhotonStream stream, PhotonMessageInfo info)
    {
        Vector3 position = (Vector3)stream.ReceiveNext();

        Quaternion rotation = (Quaternion)stream.ReceiveNext();

        if(GameSceneObjectsReferences.GameManagerBulletSerializer.BaseBulletController != null)
        {
            GameSceneObjectsReferences.GameManagerBulletSerializer.BaseBulletController.SynchedPosition = position;

            GameSceneObjectsReferences.GameManagerBulletSerializer.BaseBulletController.SynchedRotation = rotation;
        }
    }
}
