using System;

public interface IBulletVelocity<T> 
{
    Action<T> OnBulletVelocity { get; set; }
}
