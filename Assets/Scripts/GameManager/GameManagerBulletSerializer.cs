using Photon.Pun;
using UnityEngine;

public class GameManagerBulletSerializer : MonoBehaviourPun
{
    [SerializeField] BulletController _bulletController;
    [SerializeField] BulletController[] _multipleBulletsController = new BulletController[10];
    public BulletController BulletController
    {
        get => _bulletController;
        set => _bulletController = value;
    }
    public BulletController[] MultipleBulletsController
    {
        get => _multipleBulletsController;
        set => _multipleBulletsController = value;
    }


    #region Collison
    public void CallOnCollisionRPC(Collider collider, IScore iScore, int destructDamage)
    {
        if (MyPhotonNetwork.AmPhotonViewOwner(photonView))
        {
            string colliderName = collider.name;
            string ownerName = GlobalFunctions.ObjectsOfType<ScoreController>.Find(score => score.GetComponent<IScore>() == iScore).name;
            Vector3 colliderPosition = collider.transform.position;

            photonView.RPC("OnCollisionRPC", RpcTarget.AllViaServer, colliderName, ownerName, colliderPosition, destructDamage);
        }
    }

    [PunRPC]
    private void OnCollisionRPC(string colliderName, string ownerName, Vector3 colliderPosition, int destructDamage)
    {
        GlobalFunctions.Loop<Collider>.Foreach(FindObjectsOfType<Collider>(), collider => 
        {
            if (collider.name == colliderName && collider.transform.position == colliderPosition)
                Get<IDestruct>.From(collider.gameObject)?.Destruct(destructDamage, 0);
        });

        IScore iScore = GameObject.Find(ownerName)?.GetComponent<IScore>();
        iScore?.GetScore(10, null);
    }
    #endregion

    #region Damage
    public void CallDamageAndScoreRPC(IDamage iDamage, IScore iScore, int damageValue, int[] scoreValues, int damageTypeIndex)
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
                scoreValues,
                damageTypeIndex
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
            int[] scoreValues = (int[])data[3];
            iScore?.GetScore(scoreValues[0] + scoreValues[1], iDamage);
            iScore?.HitEnemyAndGetScore(scoreValues, iDamage);

            if ((int)data[4] == 1)
                iDamage.CameraChromaticAberrationFX();
        }
    }
    #endregion
}
