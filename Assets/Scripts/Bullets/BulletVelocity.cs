using UnityEngine;

public class BulletVelocity : GetBulletController
{
    private void OnEnable()
    {
        if (_bulletController != null) _bulletController.OnBulletVelocity = OnBulletVelocity;
    }

    private void OnBulletVelocity(BulletController.VelocityData velocityData)
    {
        velocityData._rigidBody.transform.rotation = velocityData._lookRotation;
        if (velocityData._isWindActivated) velocityData._rigidBody.velocity += velocityData._windVelocity;
    }
}
