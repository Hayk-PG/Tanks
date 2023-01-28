using System;
using UnityEngine;

public class BulletCollisionDirect : BulletCollision, IBulletCollisionDirect
{
    public event Action<Collider, IScore, float> onHit;


    protected override void OnCollision(Collider collider, IScore ownerScore, float distance) => onHit?.Invoke(collider, ownerScore, distance);
}