using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine;

public class GameManagerBulletSerializer : MonoBehaviourPun
{
    [SerializeField]
    private BaseBulletController _bulletController;

    [SerializeField] [Space]
    private BaseBulletController[] _multipleBulletsController = new BaseBulletController[10];

    public BaseBulletController BaseBulletController
    {
        get => _bulletController;
        set => _bulletController = value;
    }
    public BaseBulletController[] MultipleBaseBulletController
    {
        get => _multipleBulletsController;
        set => _multipleBulletsController = value;
    }

    public Action<object[]> OnTornado { get; set; }


    #region Collison
    public void CallOnCollisionRPC(Collider collider, IScore iScore, int destructDamage)
    {
        if (MyPhotonNetwork.IsMasterClient(MyPhotonNetwork.LocalPlayer))
        {
            string ownerName = GlobalFunctions.ObjectsOfType<ScoreController>.Find(sc => Get<IScore>.From(sc.gameObject) == iScore).name;

            Vector3 colliderPosition = collider.transform.position;

            photonView.RPC("OnCollisionRPC", RpcTarget.AllViaServer, ownerName, colliderPosition, destructDamage);
        }
    }

    [PunRPC]
    private void OnCollisionRPC(string ownerName, Vector3 colliderPosition, int destructDamage)
    {
        if (GameSceneObjectsReferences.TilesData.TilesDict.ContainsKey(colliderPosition))
        {
            IDestruct iDestruct = Get<IDestruct>.From(GameSceneObjectsReferences.TilesData.TilesDict[colliderPosition]);

            IScore iScore = Get<IScore>.From(GameObject.Find("ownerName") ?? gameObject);

            if (iDestruct == default)
                return;

            iDestruct.Destruct(destructDamage, 0);

            iScore?.GetScore(UnityEngine.Random.Range(10, 110), null);
        }
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
            IDamage iDamage = GameObject.Find((string)data[0]).GetComponent<IDamage>() ?? null;

            IScore iScore = GameObject.Find((string)data[1])?.GetComponent<IScore>() ?? null;

            Damage(iDamage, (int)data[2], (int)data[4]);

            Score(iScore, iDamage, (int[])data[3]);
        }
    }

    private void Damage(IDamage iDamage, int damage, int damagetypeIndex)
    {
        if (iDamage == default)
            return;

        iDamage.Damage(damage);

        if (damagetypeIndex == 1)
            iDamage.CameraChromaticAberrationFX();
    }

    private void Score(IScore iScore, IDamage iDamage, int[] scoreValues)
    {
        if (iScore == default)
            return;

        iScore.GetScore(scoreValues[0] + scoreValues[1], iDamage);
        iScore.HitEnemyAndGetScore(scoreValues, iDamage);
    }
    #endregion

    #region Tornado
    public void TornadoDamage(string damagableName, int damage, string iScoreName)
    {       
        if (MyPhotonNetwork.IsOfflineMode || !MyPhotonNetwork.IsOfflineMode && MyPhotonNetwork.IsMasterClient(MyPhotonNetwork.LocalPlayer))
        {
            EventInfo.Content_TornadoDamage = new object[]
            {
                damagableName,
                damage,
                iScoreName
            };

            //OFFLINE
            OnTornado?.Invoke(EventInfo.Content_TornadoDamage);
            //ONLINE
            PhotonNetwork.RaiseEvent(EventInfo.Code_TornadoDamage, EventInfo.Content_TornadoDamage, new RaiseEventOptions { Receivers = ReceiverGroup.All }, SendOptions.SendReliable);
        }
    }

    public void DestroyTornado(string tornadoName)
    {
        if (MyPhotonNetwork.IsMasterClient(MyPhotonNetwork.LocalPlayer))
            photonView.RPC("DestroyTornadoRPC", RpcTarget.AllViaServer, tornadoName);
    }

    [PunRPC]
    private void DestroyTornadoRPC(string tornadoName)
    {
        Tornado tornado = GameObject.Find(tornadoName)?.GetComponent<Tornado>();

        if(tornado == null)
        {
            BulletController bulletController = FindObjectOfType<BulletController>();

            if (bulletController != null)
                Destroy(bulletController.gameObject);
        }
        else
        {
            tornado.DestroyTornado();
        }
    }
    #endregion
}
