using System;

public interface IDamage
{
    int Health { get; set; }
    Action<int> OnTakeDamage { get; set; }
    Action<int> OnUpdateHealthBar { get; set; }
    void Damage(int damage);
}
