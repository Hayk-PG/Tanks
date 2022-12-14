using Photon.Pun;
using UnityEngine;

public class WoodBoxSerializeView : BaseGameManagerSerializeView
{
    private float _randomDestroyTime = 0;
    private Vector3 _rbVelocity = Vector3.zero;
    private Vector3 _rbPosition = Vector3.zero;
    private Quaternion _rbRotation = Quaternion.identity;


    protected override void Write(PhotonStream stream)
    {
        if (_woodenBoxSerializer.ParachuteWithWoodBoxController != null)
        {
            _randomDestroyTime = _woodenBoxSerializer.ParachuteWithWoodBoxController.RandomDestroyTime;
            _rbVelocity = _woodenBoxSerializer.ParachuteWithWoodBoxController.RigidBody.velocity;
            _rbRotation = _woodenBoxSerializer.ParachuteWithWoodBoxController.RigidBody.rotation;
            _rbPosition = _woodenBoxSerializer.ParachuteWithWoodBoxController.RigidBody.position;
        }

        stream.SendNext(_randomDestroyTime);
        stream.SendNext(_rbVelocity);
        stream.SendNext(_rbRotation);
        stream.SendNext(_rbPosition);
    }

    protected override void Read(PhotonStream stream, PhotonMessageInfo info)
    {
        _randomDestroyTime = (float)stream.ReceiveNext();
        _rbVelocity = (Vector3)stream.ReceiveNext();
        _rbRotation = (Quaternion)stream.ReceiveNext();
        _rbPosition = (Vector3)stream.ReceiveNext();

        if (_woodenBoxSerializer.ParachuteWithWoodBoxController != null)
        {
            _woodenBoxSerializer.ParachuteWithWoodBoxController.RandomDestroyTime = _randomDestroyTime;
            _woodenBoxSerializer.ParachuteWithWoodBoxController.RigidBody.velocity = _rbVelocity;
            _woodenBoxSerializer.ParachuteWithWoodBoxController.RigidBody.rotation = _rbRotation;
            _woodenBoxSerializer.ParachuteWithWoodBoxController.RigidBody.position = _rbPosition + _rbVelocity * Lag(info);
        }
    }
}
