using System;
using UnityEngine;

public interface IBulletCollision 
{
    Action<Collision, IScore> OnCollision { get; set; }
    Action<IScore> OnExplodeOnCollision { get; set; }
}
