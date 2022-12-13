using Photon.Pun;
using UnityEngine;

public class GlobalExplosiveBarrels : MonoBehaviourPun
{
    public Rigidbody[] BarrelRigidBody { get; set; } = new Rigidbody[4];
    private GameObject _tempBarrel;


    public void LaunchBarrel(Vector3 position)
    {
        if (MyPhotonNetwork.IsMasterClient(MyPhotonNetwork.LocalPlayer))
            photonView.RPC("LaunchBarrelRPC", RpcTarget.AllViaServer, position);
    }

    [PunRPC]
    private void LaunchBarrelRPC(Vector3 position)
    {
        ExplosiveBarrels[] explosiveBarrels = FindObjectsOfType<ExplosiveBarrels>();

        GlobalFunctions.Loop<ExplosiveBarrels>.Foreach(explosiveBarrels, explosiveBarrel =>
        {
            if (explosiveBarrel.transform.position == position)
            {
                explosiveBarrel.LaunchBarrel();
            }
        });
    }

    public void AllocateBarrel(Vector3 id)
    {
        if (MyPhotonNetwork.IsMasterClient(MyPhotonNetwork.LocalPlayer))
            photonView.RPC("AllocateBarrelRPC", RpcTarget.AllViaServer, id);
    }

    [PunRPC]
    private void AllocateBarrelRPC(Vector3 id)
    {
        for (int i = 0; i < BarrelRigidBody.Length; i++)
        {
            if(BarrelRigidBody[i] == null)
            {
                _tempBarrel = GlobalFunctions.ObjectsOfType<Barrel>.Find(barrel => barrel.ID == id)?.gameObject;
                BarrelRigidBody[i] = _tempBarrel != null ? Get<Rigidbody>.From(_tempBarrel) : null;
            }
        }
    }

    public void ExplodeBarrel(string collisionTag, Vector3 collisionPosition, Vector3 id)
    {
        photonView.RPC("ExplodeBarrelRPC", RpcTarget.AllViaServer, collisionTag, collisionPosition, id);
    }

    [PunRPC]
    private void ExplodeBarrelRPC(string collisionTag, Vector3 collisionPosition, Vector3 id)
    {
        GlobalFunctions.ObjectsOfType<Barrel>.Find(barrel => barrel.ID == id)?.Damage(collisionTag, collisionPosition);
    }
}
