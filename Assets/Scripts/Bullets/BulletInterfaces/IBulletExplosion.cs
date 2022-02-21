using System;

public interface IBulletExplosion 
{
    Action<IScore> OnBulletExplosion { get; set; }
}
