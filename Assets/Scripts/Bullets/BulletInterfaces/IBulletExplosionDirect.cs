using System;
using UnityEngine;

public interface IBulletExplosionDirect
{
    event Action<Collider, IScore, float> onBulletExplosion;
}
