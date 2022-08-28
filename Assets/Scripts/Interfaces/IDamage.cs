using System;

public interface IDamage
{
    int Health { get; set; }
    Action<BasePlayer, int> OnTakeDamage { get; set; }
    Action<int> OnUpdateHealthBar { get; set; }
    void Damage(int damage);
    void CameraChromaticAberrationFX();
}
