using Photon.Pun;
using UnityEngine;

public class PlatformsSerializeView : BaseGameManagerSerializeView
{
    [SerializeField] private Platform _horizPlatform;
    [SerializeField] private Platform _vertPlatform;

    protected override void Write(PhotonStream stream)
    {
        stream.SendNext(_horizPlatform.transform.position);
        stream.SendNext(_horizPlatform.transform.rotation);

        stream.SendNext(_vertPlatform.transform.position);
        stream.SendNext(_vertPlatform.transform.rotation);
    }

    protected override void Read(PhotonStream stream, PhotonMessageInfo info)
    {
        _horizPlatform.transform.position = (Vector3)stream.ReceiveNext();
        _horizPlatform.transform.rotation = (Quaternion)stream.ReceiveNext();

        _vertPlatform.transform.position = (Vector3)stream.ReceiveNext();
        _vertPlatform.transform.rotation = (Quaternion)stream.ReceiveNext();
    }
}
