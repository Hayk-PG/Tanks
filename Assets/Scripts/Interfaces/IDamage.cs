using System;
using UnityEngine;

public interface IDamage
{
    int Health { get; set; }
    Action<BasePlayer, int> OnTakeDamage { get; set; }
    Action<int> OnUpdateHealthBar { get; set; }
    void Damage(int damage, bool ingoreArmor = false);
    void Damage(Collider collider, int damage, bool ignoreArmor = false);
    void CameraChromaticAberrationFX();
}
