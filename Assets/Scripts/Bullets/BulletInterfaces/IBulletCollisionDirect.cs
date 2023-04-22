using System;
using UnityEngine;

public interface IBulletCollisionDirect 
{
    event Action<Collider, IScore, float> onHit;
}
