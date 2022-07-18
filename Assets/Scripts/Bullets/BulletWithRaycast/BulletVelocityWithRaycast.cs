using UnityEngine;

public class BulletVelocityWithRaycast : BulletVelocity
{
    protected override Vector3 ExtraMovementValue
    {
        get
        {
            return new Vector3(0, 2 * Time.fixedDeltaTime, 0);
        }
    }
}
