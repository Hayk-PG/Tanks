using UnityEngine;

public class BulletCollision : GetBulletController
{
    [SerializeField] private int _destructDamage;
    private int _collisionCount;

    private void OnEnable()
    {
        _iBulletCollision.OnCollision = OnCollision;
    }

    private void OnCollision(Collision collision, IScore ownerScore)
    {
        _collisionCount++;

        if (_collisionCount <= 1)
            Conditions<bool>.Compare(MyPhotonNetwork.IsOfflineMode, () => OnCollisionInOfflineMode(collision, ownerScore), () => OnCollisionInOnlineMode(collision, ownerScore));
    }

    private void OnCollisionInOfflineMode(Collision collision, IScore ownerScore)
    {
        ownerScore.GetScore(10, null);
        Get<IDestruct>.From(collision.gameObject)?.Destruct(_destructDamage);
    }

    private void OnCollisionInOnlineMode(Collision collision, IScore ownerScore)
    {
        _gameManagerBulletSerializer.CallOnCollisionRPC(collision, ownerScore, _destructDamage);
    }
}
