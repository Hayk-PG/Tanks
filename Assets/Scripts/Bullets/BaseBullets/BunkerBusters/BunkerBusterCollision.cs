using UnityEngine;

public class BunkerBusterCollision : BulletSensorCollision
{
    private GameObject _collision;

    private Vector3 _collisionPosition;

    [SerializeField] [Space]
    private int _maxCollisionsCount;
    private int _collisionsCount;


    protected override void OnHit(RaycastHit raycastHit)
    {
        if (_collision == raycastHit.collider.gameObject || _collisionPosition == raycastHit.collider.transform.position || _isCollided)
            return;

        _collision = raycastHit.collider.gameObject;

        _collisionPosition = raycastHit.collider.transform.position;

        if (_collision.tag == Tags.Player || _collision.tag == Tags.AI)
        {
            OnCollision(raycastHit.collider);

            _isCollided = true;

            return;
        }
        else
        {
            _collisionsCount++;

            if (_collisionsCount >= _maxCollisionsCount)
            {
                OnCollision(raycastHit.collider);

                _isCollided = true;
            }
        }
    }

    protected override void OnCollision(Collider collider)
    {
        RaiseOnCollision(collider);

        base.OnCollision(collider);
    }
}
