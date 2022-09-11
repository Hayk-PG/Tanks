using UnityEngine;

public class BulletCollision : GetBulletController
{
    [HideInInspector]
    [SerializeField]
    protected int _destructDamage, _tileParticleIndex;

    public int DestructDamage
    {
        get => _destructDamage;
        set => _destructDamage = value;
    }
    public int TileParticleIndex
    {
        get => _tileParticleIndex;
        set => _tileParticleIndex = value;
    }


    protected virtual void OnEnable()
    {
        _iBulletCollision.OnCollision += OnCollision;
    }

    protected virtual void OnDisable()
    {
        _iBulletCollision.OnCollision -= OnCollision;
    }

    protected virtual void OnCollision(Collider collider, IScore ownerScore, float distance)
    {
        if (Get<TankController>.From(collider.gameObject) != null)
            SecondarySoundController.PlaySound(0, 2);

        Conditions<bool>.Compare(MyPhotonNetwork.IsOfflineMode, () => OnCollisionInOfflineMode(collider, ownerScore), () => OnCollisionInOnlineMode(collider, ownerScore));       
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
