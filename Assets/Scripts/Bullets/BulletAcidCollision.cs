using UnityEngine;

public class BulletAcidCollision : BulletSensorCollision
{
    protected override void OnHit(RaycastHit raycastHit)
    {
        if (!_isCollided)
        {
            RaiseOnCollision(raycastHit.collider);

            _isCollided = true;
        }
    }
}
