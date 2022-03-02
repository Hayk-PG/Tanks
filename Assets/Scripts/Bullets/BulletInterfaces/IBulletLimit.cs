using System;

public interface IBulletLimit 
{
    Action<bool> OnExplodeOnLimit { get; set; }
}
