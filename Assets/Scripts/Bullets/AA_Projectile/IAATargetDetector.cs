using System;

public interface IAATargetDetector<T> 
{
    BulletController Target { get; set; }

    public Action OnTargetDetected { get; set; }

    void ScanTarget(bool isConditionMet);

    void MoveTowardsTarget(bool isConditionMet, T parameter);

    void DestroyTarget(bool isConditionMet);
}
