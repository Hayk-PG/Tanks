using UnityEngine;

public class BunkerBusterCollision : BaseBulletCollision
{
    protected GameObject _collision;

    [SerializeField] [Space]
    protected int _maxCollisionsCount;
    protected int _collisionsCount;

    [SerializeField] [Space]
    protected bool _destroyPenetratedTiles;




    protected override void OnCollisionEnter(Collision collision)
    {
        
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (_collision == other.gameObject || _isCollided)
            return;

        _collision = other.gameObject;

        if (other.tag == Tags.Player || other.tag == Tags.AI)
        {
            OnCollision(other);

            return;
        }

        PenetrateThroughSeveralTiles(other);
    }

    protected virtual void PenetrateThroughSeveralTiles(Collider other)
    {
        _collisionsCount++;

        if (_collisionsCount < _maxCollisionsCount)
        {
            if (_destroyPenetratedTiles)
                base.OnCollision(other);
        }
        else
        {
            OnCollision(other);
        }
    }

    protected override void OnCollision(Collider collider)
    {
        RaiseOnCollision(collider);

        base.OnCollision(collider);

        _isCollided = true;
    }
}
