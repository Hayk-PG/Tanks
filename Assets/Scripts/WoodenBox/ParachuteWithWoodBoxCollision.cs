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

    internal Action<CollisionData> OnCollision { get; set; }


    private void OnCollisionEnter(Collision collision)
    {
        OnCollision?.Invoke(new CollisionData 
        { 
            _collision = collision,
            _tankController = Get<TankController>.From(collision.gameObject),
            _bulletController = Get<BulletController>.From(collision.gameObject)
    });
    }
}
