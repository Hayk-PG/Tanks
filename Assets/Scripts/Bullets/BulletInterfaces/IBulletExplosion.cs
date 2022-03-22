using System;

public interface IBulletExplosion 
{
    Action<IScore> OnBulletExplosion { get; set; }
    Action OnBulletExplosionWithoutHitting { get; set; }
}
