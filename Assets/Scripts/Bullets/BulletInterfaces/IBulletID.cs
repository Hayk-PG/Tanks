using UnityEngine;

public interface IBulletID 
{
    abstract Rigidbody RigidBody { get; set; }
    abstract IScore OwnerScore { get; set; }
    abstract Vector3 StartPosition { get; set; }
    abstract float Distance { get; }
    abstract bool IsLastShellOfBarrage { get; set; }
    abstract TurnController TurnController { get; set; }
}
