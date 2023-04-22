using Photon.Pun;
using UnityEngine;

public class WoodBoxSerializeView : BaseGameManagerSerializeView
{
    protected override void Write(PhotonStream stream)
    {
        if (_woodenBoxSerializer.ParachuteWithWoodBoxController != null)
        {
            stream.SendNext(_woodenBoxSerializer.ParachuteWithWoodBoxController.RigidBody.velocity);
            stream.SendNext(_woodenBoxSerializer.ParachuteWithWoodBoxController.RigidBody.position);
            stream.SendNext(_woodenBoxSerializer.ParachuteWithWoodBoxController.RigidBody.rotation);          
        }
    }

    protected override void Read(PhotonStream stream, PhotonMessageInfo info)
    {
        if (_woodenBoxSerializer.ParachuteWithWoodBoxController != null)
        {
            _woodenBoxSerializer.ParachuteWithWoodBoxController.RigidBody.velocity = (Vector3)stream.ReceiveNext();
            _woodenBoxSerializer.ParachuteWithWoodBoxController.RigidBody.position = (Vector3)stream.ReceiveNext();
            _woodenBoxSerializer.ParachuteWithWoodBoxController.RigidBody.rotation = (Quaternion)stream.ReceiveNext();
            _woodenBoxSerializer.ParachuteWithWoodBoxController.RigidBody.position += _woodenBoxSerializer.ParachuteWithWoodBoxController.RigidBody.velocity * Lag(info);
        }
    }
}
