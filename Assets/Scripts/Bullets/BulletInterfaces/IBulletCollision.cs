using System;
using UnityEngine;

public interface IBulletCollision 
{
    Action<Collision> OnCollision { get; set; }
    Action<IScore> OnExplodeOnCollision { get; set; }
}
