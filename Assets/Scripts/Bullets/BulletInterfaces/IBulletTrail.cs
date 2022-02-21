using System;

public interface IBulletTrail 
{
    Action<bool> OnTrailActivity { get; set; }
}
