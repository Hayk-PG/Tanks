using System;

public interface IDamage
{
    int Health { get; set; }
    Action OnTakeDamage { get; set; }
    void Damage(int damage);
}
