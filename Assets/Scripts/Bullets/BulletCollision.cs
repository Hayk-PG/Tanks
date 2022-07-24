using UnityEngine;

public class BulletCollision : GetBulletController
{
    [SerializeField] private int _destructDamage;
    private int _collisionCount;

    private void OnEnable()
    {
        _iBulletCollision.OnCollision = OnCollision;
    }

    private void OnCollision(Collider collider, IScore ownerScore, float distance)
    {
        _collisionCount++;

        if (_collisionCount <= 1)
        {
            Conditions<bool>.Compare(MyPhotonNetwork.IsOfflineMode, () => OnCollisionInOfflineMode(collider, ownerScore), () => OnCollisionInOnlineMode(collider, ownerScore));
        }           
    }

    private void OnCollisionInOfflineMode(Collider collider, IScore ownerScore)
    {
        ownerScore.GetScore(10, null);
        Get<IDestruct>.From(collider.gameObject)?.Destruct(_destructDamage, 0);
    }

    private void OnCollisionInOnlineMode(Collider collider, IScore ownerScore)
    {
        _gameManagerBulletSerializer.CallOnCollisionRPC(collider, ownerScore, _destructDamage);
    }
}
