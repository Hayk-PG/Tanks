using System.Collections;
using UnityEngine;

public class RailgunCollision : BaseBulletCollision
{
    protected int _tileCollisionCount;



    protected virtual void OnTriggerEnter(Collider other)
    {
        bool canCollideWithTile = other.tag == Tags.Tile && _tileCollisionCount < 1;

        // Checking if the first collision is with a Tile. If it is, destroy the Tile without destroying the projectile.

        if (canCollideWithTile)
        {
            OnCollision(other);

            _tileCollisionCount++;

            return;
        }

        // If the collision is not with a Tile, the 'RaiseOnCollision' method will be called.
        // If it collides with a Tile for the second time, the 'OnCollision' method will also be called.

        OnCollision(other);

        RaiseOnCollision(other);
    }

    protected virtual void OnTriggerExit(Collider other) => StartCoroutine(RaiseOnCollisionAfterDelay(other));

    // If no collision is detected immediately after the first collision, call the 'RaiseOnCollision' method to explode and destroy the projectile.

    protected IEnumerator RaiseOnCollisionAfterDelay(Collider other)
    {
        yield return null;

        print("OnTriggerExit");

        RaiseOnCollision(other);
    }

    protected override void RaiseOnCollision(Collider collider)
    {
        print("RaiseOnCollision");

        base.RaiseOnCollision(collider);
    }
}
