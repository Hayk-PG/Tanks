using System;
public interface ILockedMissile 
{
    Action<float> OnTargetLocked { get; set; }
}
