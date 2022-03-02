using System;

public class BulletVelocity : GetBulletController, IBulletTrail
{
    public Action<bool> OnTrailActivity { get; set; }


    protected virtual void Start()
    {
        Invoke("ActivateTrail", 0.1f);
    }

    protected virtual void OnEnable()
    {
        if (_iBulletVelocity != null) _iBulletVelocity.OnBulletVelocity = OnBulletVelocity;
    }

    protected virtual void ActivateTrail()
    {
        OnTrailActivity?.Invoke(true);
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
