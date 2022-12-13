using Photon.Pun;
using UnityEngine;

public class GlobalExplosiveBarrels : MonoBehaviourPun
{
    public Rigidbody[] BarrelRigidBody { get; set; } = new Rigidbody[4];


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

    public void AllocateBarrel(Vector3 position)
    {
        if (MyPhotonNetwork.IsMasterClient(MyPhotonNetwork.LocalPlayer))
            photonView.RPC("AllocateBarrelRPC", RpcTarget.AllViaServer, position);
    }

    [PunRPC]
    private void AllocateBarrelRPC(Vector3 position)
    {
        Barrel[] barrels = FindObjectsOfType<Barrel>();

        GlobalFunctions.Loop<Barrel>.Foreach(barrels, barrel =>
        {
            if (Get<Rigidbody>.From(barrel.gameObject).position == position)
            {
                for (int i = 0; i < BarrelRigidBody.Length; i++)
                {
                    if(BarrelRigidBody[i] == null)
                    {
                        BarrelRigidBody[i] = Get<Rigidbody>.From(barrel.gameObject);
                        break;
                    }
                }
            } 
        });
    }

    public void ExplodeBarrel(string collisionTag, Vector3 collisionPosition, Vector3 barrelRigidbodyPosition)
    {
        photonView.RPC("ExplodeBarrelRPC", RpcTarget.AllViaServer, collisionTag, collisionPosition, barrelRigidbodyPosition);
    }

    [PunRPC]
    private void ExplodeBarrelRPC(string collisionTag, Vector3 collisionPosition, Vector3 barrelRigidbodyPosition)
    {
        Barrel barrel = GlobalFunctions.ObjectsOfType<Barrel>.Find(b => b.Rigidbody.position == barrelRigidbodyPosition);
        barrel.Damage(collisionTag, collisionPosition);
    }
}
