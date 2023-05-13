using UnityEngine;
using System;

public class BaseBulletCollision : MonoBehaviour
{
    [SerializeField]
    protected BaseBulletController _baseBulletController;

    [SerializeField] [Space]
    protected int _destructDamage, _tileParticleIndex;

    protected bool _isCollided;

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

    public event Action<Collider> onCollision;



    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (!_isCollided)
        {
            RaiseOnCollision(collision.collider);

            OnCollision(collision.collider);

            _isCollided = true;
        }
    }

    protected virtual void RaiseOnCollision(Collider collider)
    {
        onCollision?.Invoke(collider);
    }

    protected virtual void OnCollision(Collider collider)
    {
        Conditions<bool>.Compare(MyPhotonNetwork.IsOfflineMode, () => OnCollisionInOfflineMode(collider), ()=> OnCollisionInOnlineMode(collider));
    }

    protected virtual void OnCollisionInOfflineMode(Collider collider)
    {
        DestroyTile(collider);
    }

    protected virtual void OnCollisionInOnlineMode(Collider collider)
    {
        GameSceneObjectsReferences.GameManagerBulletSerializer.CallOnCollisionRPC(collider, _baseBulletController.OwnerScore, DestructDamage);
    }

    protected virtual void DestroyTile(Collider collider)
    {
        IDestruct iDestruct = Get<IDestruct>.From(collider.gameObject);

        if(iDestruct == default)
            return;

        iDestruct.Destruct(_destructDamage, _tileParticleIndex);

        _baseBulletController?.OwnerScore.GetScore(UnityEngine.Random.Range(10, 110), null, transform.position);
    }
}
