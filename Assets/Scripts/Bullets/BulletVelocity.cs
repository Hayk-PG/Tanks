using UnityEngine;

public class BulletVelocity : GetBulletController
{
    protected virtual void OnEnable()
    {
        if (_bulletController != null) _bulletController.OnBulletVelocity = OnBulletVelocity;
    }

    protected virtual void OnBulletVelocity(BulletController.VelocityData velocityData)
    {
        BulletLookRotation(velocityData);
        ApplyWindForceToTheMovement(velocityData);
    }

    protected virtual void BulletLookRotation(BulletController.VelocityData velocityData)
    {
        velocityData._rigidBody.transform.rotation = velocityData._lookRotation;
    }

    protected virtual void ApplyWindForceToTheMovement(BulletController.VelocityData velocityData)
    {
        if (velocityData._isWindActivated)
            velocityData._rigidBody.velocity += velocityData._windVelocity;
    }
}
