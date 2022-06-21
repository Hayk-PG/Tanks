using Photon.Pun;
using UnityEngine;

public class GameManagerBulletSerializer : MonoBehaviourPun
{
    [SerializeField] BulletController _bulletController;
    public BulletController BulletController
    {
        get => _bulletController;
        set => _bulletController = value;
    }


    public void CallOnCollisionRPC(Collision collision, IScore iScore, int destructDamage)
    {
        if (MyPhotonNetwork.AmPhotonViewOwner(photonView))
        {
            string collisionName = collision.collider.name;
            string ownerName = GlobalFunctions.ObjectsOfType<ScoreController>.Find(score => score.GetComponent<IScore>() == iScore).name;
            Vector3 collisionPosition = collision.collider.transform.position;

            photonView.RPC("OnCollisionRPC", RpcTarget.AllViaServer, collisionName, ownerName, collisionPosition, destructDamage);
        }
    }

    [PunRPC]
    private void OnCollisionRPC(string collisionName, string ownerName, Vector3 collisionPosition, int destructDamage)
    {
        GlobalFunctions.Loop<Collider>.Foreach(FindObjectsOfType<Collider>(), collision => 
        {
            if (collision.name == collisionName && collision.transform.position == collisionPosition)
                Get<IDestruct>.From(collision.gameObject)?.Destruct(destructDamage);
        });

        IScore iScore = GameObject.Find(ownerName)?.GetComponent<IScore>();
        iScore?.GetScore(10, null);

        print("(BulletCollision) " + collisionName + "/" + ownerName + "/" + collisionPosition);
    }

    public void CallDamageAndScoreRPC(IDamage iDamage, IScore iScore, int damageValue, int scoreValue, int hitEnemyAndGetScoreValue)
    {
        if (MyPhotonNetwork.AmPhotonViewOwner(photonView))
        {
            string iDamageOwnerName = GlobalFunctions.ObjectsOfType<HealthController>.Find(health => health.GetComponent<IDamage>() == iDamage).name;
            string ownerName = GlobalFunctions.ObjectsOfType<ScoreController>.Find(score => score.GetComponent<IScore>() == iScore).name;
            object[] data = new object[]
            {
                iDamageOwnerName,
                ownerName,
                damageValue,
                scoreValue,
                hitEnemyAndGetScoreValue
            };

            photonView.RPC("DamageAndScoreRPC", RpcTarget.AllViaServer, data);
        }
    }

    [PunRPC]
    private void DamageAndScoreRPC(object[] data)
    {
        if(data != null)
        {
            IDamage iDamage = GameObject.Find((string)data[0])?.GetComponent<IDamage>();
            IScore iScore = GameObject.Find((string)data[1])?.GetComponent<IScore>();

            iDamage?.Damage((int)data[2]);
            iScore?.GetScore((int)data[3], iDamage);
            iScore?.HitEnemyAndGetScore((int)data[4], iDamage);

            print("(Explosion) " + (string)data[0] + "/" + (int)data[2] + "/" + (string)data[1] + "/" + (int)data[3] + "/" + (int)data[4]);
        }
    }
}
