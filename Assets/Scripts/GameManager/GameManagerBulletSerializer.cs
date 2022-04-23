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


    public void CallOnCollisionRPC(Collision collision, IScore iScore)
    {
        if (MyPhotonNetwork.AmPhotonViewOwner(photonView))
        {
            string collisionName = collision.collider.name;
            string ownerName = GlobalFunctions.ObjectsOfType<ScoreController>.Find(score => score.GetComponent<IScore>() == iScore).name;
            Vector3 collisionPosition = collision.collider.transform.position;

            photonView.RPC("OnCollisionRPC", RpcTarget.AllViaServer, collisionName, ownerName, collisionPosition);
        }
    }

    [PunRPC]
    private void OnCollisionRPC(string collisionName, string ownerName, Vector3 collisionPosition)
    {
        GlobalFunctions.Loop<Collider>.Foreach(FindObjectsOfType<Collider>(), collision => 
        {
            if (collision.name == collisionName && collision.transform.position == collisionPosition)
                Get<IDestruct>.From(collision.gameObject)?.Destruct();
        });

        IScore iScore = GameObject.Find(ownerName)?.GetComponent<IScore>();
        iScore?.GetScore(10, null);
    }
}
