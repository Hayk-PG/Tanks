using Photon.Pun;
using UnityEngine;

public class GlobalExplosiveBarrels : MonoBehaviourPun
{
    public Rigidbody BarrelRigidBody { get; set; }


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
                BarrelRigidBody = Get<Rigidbody>.From(barrel.gameObject);
        });
    }
}
