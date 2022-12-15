using System;
using UnityEngine;

public interface IBulletSensor 
{
    public Action<RaycastHit> OnHit { get; set; }
}
