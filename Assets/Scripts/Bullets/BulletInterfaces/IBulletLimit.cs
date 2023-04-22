using System;

public interface IBulletLimit 
{
    Action<bool> OnExplodeOnLimit { get; set; }
    Action OnDestroyTimeLimit { get; set; }
}
