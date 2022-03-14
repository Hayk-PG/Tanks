using UnityEngine;

public class BulletCollision : GetBulletController
{
    private int _collisionCount;


    private void OnEnable()
    {
        _iBulletCollision.OnCollision = OnCollision;
    }

    private void OnCollision(Collision collision, IScore ownerScore)
    {
        _collisionCount++;

        if (_collisionCount <= 1)
        {
            ownerScore.OnDisplayTemPoints?.Invoke(10, ownerScore.PlayerTurn.MyTurn, collision.transform.position);
            Get<IDestruct>.From(collision.gameObject)?.Destruct();           
        }
    }
}
