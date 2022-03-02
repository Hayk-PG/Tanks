using UnityEngine;

public class BulletCollision : GetBulletController
{
    private int _collisionCount;

    private void OnEnable()
    {
        _iBulletCollision.OnCollision = OnCollision;
    }

    private void OnCollision(Collision collision)
    {
        _collisionCount++;

        if (_collisionCount <= 1) Get<IDestruct>.From(collision.gameObject)?.Destruct();
    }
}
