using System;

public interface IBulletExplosion 
{
    Action<IScore, float> OnBulletExplosion { get; set; }
    Action OnBulletExplosionWithoutHitting { get; set; }
}
