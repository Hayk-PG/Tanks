using UnityEngine;


public class BombVelocity : BaseBulletVelocity
{
    protected override void Start()
    {
        
    }

    protected override void ControlMovement()
    {
        _baseBulletController.RigidBody.velocity += Vector3.down * 2.5f * Time.fixedDeltaTime;
    }

    protected override void ControlGravitation()
    {
        
    }
}
