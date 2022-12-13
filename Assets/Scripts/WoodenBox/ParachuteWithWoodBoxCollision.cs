using System;
using UnityEngine;


public class ParachuteWithWoodBoxCollision : MonoBehaviour
{
    internal struct CollisionData
    {
        internal Collision _collision;
        internal TankController _tankController;
        internal BulletController _bulletController;
    }

    internal event Action<CollisionData> onCollisionEnter;


    private void OnCollisionEnter(Collision collision)
    {
        onCollisionEnter?.Invoke(new CollisionData
        {
            _collision = collision,
            _tankController = Get<TankController>.From(collision.gameObject),
            _bulletController = Get<BulletController>.From(collision.gameObject)
        });
    }
}
