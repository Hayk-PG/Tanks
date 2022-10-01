using System;
using UnityEngine;

public class TornadoShellParticles : FlareBulletParticles
{
    protected override void OnCollision(RaycastHit hit, IScore ownerScore, float distance)
    {
        ActivateExplosion(hit);
        _explosion.OwnerScore = ownerScore;
        Destroy(gameObject);
    }
}
