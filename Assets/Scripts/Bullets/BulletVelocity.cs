using System;
using UnityEngine;

public class BulletVelocity : GetBulletController, IBulletTrail
{
    public Action<bool> OnTrailActivity { get; set; }
    protected float _gravity;

    protected virtual Vector3 ExtraMovementValue
    {
        get
        {
            return Vector3.zero;
        }
    }



    protected virtual void Start()
    {
        Invoke("ActivateTrail", 0.1f);
    }

    protected virtual void OnEnable()
    {
        if (_iBulletVelocity != null) 
            _iBulletVelocity.OnBulletVelocity += OnBulletVelocity;
    }

    protected virtual void OnDisable()
    {
        if (_iBulletVelocity != null) 
            _iBulletVelocity.OnBulletVelocity -= OnBulletVelocity;
    }

    protected virtual void ActivateTrail() => OnTrailActivity?.Invoke(true);

    protected virtual void OnBulletVelocity(BulletController.VelocityData velocityData)
    {
        BulletLookRotation(velocityData);
        ApplyWindForceToTheMovement(velocityData);
        IncreaseGravitation(velocityData, 1.5f);
    }

    protected virtual void IncreaseGravitation(BulletController.VelocityData velocityData, float force)
    {
        if (velocityData._rigidBody.velocity.y <= 0)
        {
            _gravity = velocityData._rigidBody.velocity.y + velocityData._rigidBody.velocity.y * force * Time.fixedDeltaTime;
            velocityData._rigidBody.velocity = new Vector3(velocityData._rigidBody.velocity.x, _gravity, velocityData._rigidBody.velocity.z);
        }
    }

    protected virtual void BulletLookRotation(BulletController.VelocityData velocityData) => velocityData._rigidBody.transform.rotation = velocityData._lookRotation;

    protected virtual void ApplyWindForceToTheMovement(BulletController.VelocityData velocityData)
    {
        if (velocityData._isWindActivated)
            velocityData._rigidBody.velocity += velocityData._windVelocity + ExtraMovementValue;
        else
            velocityData._rigidBody.velocity += ExtraMovementValue;
    }
}
