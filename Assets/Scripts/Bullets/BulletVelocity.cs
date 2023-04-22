using System;
using UnityEngine;

public class BulletVelocity : GetBulletController, IBulletTrail
{
    [SerializeField] protected float _gravityForcePercentage;
    [SerializeField] protected float _windForcePercentage;  
    protected float _gravity;
    
    public float GravityForcePercentage { get => _gravityForcePercentage; set => _gravityForcePercentage = value; }
    public float WindForcePercentage { get => _windForcePercentage; set => _windForcePercentage = value; }

    public Action<bool> OnTrailActivity { get; set; }
    

    

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
        IncreaseGravitation(velocityData, GravityForcePercentage);
    }

    protected virtual void IncreaseGravitation(BulletController.VelocityData velocityData, float force)
    {
        if (velocityData._rigidBody.velocity.y <= 0)
        {
            _gravity = velocityData._rigidBody.velocity.y + (velocityData._rigidBody.velocity.y / 100 * force) * Time.fixedDeltaTime;
            velocityData._rigidBody.velocity = new Vector3(velocityData._rigidBody.velocity.x, _gravity, velocityData._rigidBody.velocity.z);
        }
    }

    protected virtual void BulletLookRotation(BulletController.VelocityData velocityData) => velocityData._rigidBody.transform.rotation = velocityData._lookRotation;

    protected virtual void ApplyWindForceToTheMovement(BulletController.VelocityData velocityData)
    {
        if (velocityData._isWindActivated)
            velocityData._rigidBody.velocity += new Vector3((velocityData._windVelocity.x / 100 * WindForcePercentage), velocityData._windVelocity.y, velocityData._windVelocity.z);
    }
}
