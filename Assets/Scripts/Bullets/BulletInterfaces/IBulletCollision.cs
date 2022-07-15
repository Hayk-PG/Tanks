using System;
using UnityEngine;

public interface IBulletCollision 
{
    Action<Collider, IScore> OnCollision { get; set; }
    Action<IScore> OnExplodeOnCollision { get; set; }
}
