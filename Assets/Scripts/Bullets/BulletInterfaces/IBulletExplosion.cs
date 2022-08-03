using System;
using UnityEngine;

public interface IBulletExplosion 
{
    Action<IScore, float> OnBulletExplosion { get; set; }
    Action<IScore, float, Vector3?> OnFlareBulletExplosion { get; set; }
    Action OnBulletExplosionWithoutHitting { get; set; }
}
