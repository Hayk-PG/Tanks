using UnityEngine;
using Photon.Pun;

public class PhotonNetworkAALauncher : MonoBehaviourPun
{
    public void ActivateAALauncher(Vector3 tilePosition, string tankName)
    {
        if (MyPhotonNetwork.IsMasterClient(MyPhotonNetwork.LocalPlayer))
            photonView.RPC("ActivateAALauncherRPC", RpcTarget.AllViaServer, tilePosition, tankName);
    }

    [PunRPC]
    private void ActivateAALauncherRPC(Vector3 tilePosition, string tankName)
    {
        GameObject tile = FindObjectOfType<TilesData>().TilesDict[tilePosition];

        if(tile != null)
        {
            TileProps tileProps = Get<TileProps>.From(tile);
            tileProps.ActiveProps(TileProps.PropsType.AALauncher, true, tankName == Names.Tank_FirstPlayer);
        }  
    }

    public void InitMissile(Vector3 id)
    {
        if (MyPhotonNetwork.IsMasterClient(MyPhotonNetwork.LocalPlayer))
            photonView.RPC("InitMissileeRPC", RpcTarget.AllViaServer, id);
    }

    [PunRPC]
    private void InitMissileeRPC(Vector3 id)
    {
        AAProjectileLauncher aAProjectileLauncher = GlobalFunctions.ObjectsOfType<AAProjectileLauncher>.Find(launcher => launcher.ID == id);
        aAProjectileLauncher?.InitMissile();
    }

    public void DestroyTarget(string enemyName)
    {
        if (MyPhotonNetwork.IsMasterClient(MyPhotonNetwork.LocalPlayer))
            photonView.RPC("DestroyTargetRPC", RpcTarget.AllViaServer, enemyName);
    }

    [PunRPC]
    private void DestroyTargetRPC(string enemyName)
    {
        if(enemyName != null)
        {
            BulletController Target = GlobalFunctions.ObjectsOfType<BulletController>.Find(bullet => bullet.OwnerScore.PlayerTurn.gameObject.name == enemyName);

            if (Target != null)
            {
                IBulletExplosion iBulletExplosion = Get<IBulletExplosion>.From(Target.gameObject);
                iBulletExplosion?.DestroyBullet();
            }
        }
    }
}
