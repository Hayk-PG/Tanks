using UnityEngine;

public class BulletCollision : GetBulletController
{
    [SerializeField] protected int _destructDamage;
    [SerializeField] protected int _tileParticleIndex;
    protected int _collisionCount;

    protected virtual void OnEnable()
    {
        _iBulletCollision.OnCollision = OnCollision;
    }

    protected virtual void OnCollision(Collider collider, IScore ownerScore, float distance)
    {
        _collisionCount++;

        if (_collisionCount <= 1)
        {
            Conditions<bool>.Compare(MyPhotonNetwork.IsOfflineMode, () => OnCollisionInOfflineMode(collider, ownerScore), () => OnCollisionInOnlineMode(collider, ownerScore));
        }           
    }

    protected virtual void OnCollisionInOfflineMode(Collider collider, IScore ownerScore)
    {
        ownerScore.GetScore(10, null);
        Get<IDestruct>.From(collider.gameObject)?.Destruct(_destructDamage, _tileParticleIndex);
    }

    protected virtual void OnCollisionInOnlineMode(Collider collider, IScore ownerScore)
    {
        _gameManagerBulletSerializer.CallOnCollisionRPC(collider, ownerScore, _destructDamage);
    }
}
