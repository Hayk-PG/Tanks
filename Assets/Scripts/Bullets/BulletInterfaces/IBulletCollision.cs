using System;
using UnityEngine;

public interface IBulletCollision 
{
    Action<Collider, IScore, float> OnCollision { get; set; }
    Action<IScore, float> OnExplodeOnCollision { get; set; }
}
