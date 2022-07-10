using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtilleryBulletVelocity : BulletVelocity
{
    protected override void OnBulletVelocity(BulletController.VelocityData velocityData)
    {
        BulletLookRotation(velocityData);
        IncreaseGravitation(velocityData, 1.5f);
    }
}
