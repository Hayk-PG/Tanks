using System;
using UnityEngine;

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

        if(velocityData._rigidBody.velocity.y <= 0)
        {
            float y = velocityData._rigidBody.velocity.y + velocityData._rigidBody.velocity.y * 10 * Time.fixedDeltaTime;
            velocityData._rigidBody.velocity = new Vector3(velocityData._rigidBody.velocity.x, y, velocityData._rigidBody.velocity.z);
        }
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
