using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable objects/Weapons/New weapon (DIRECT)")]
public class WeaponPropertiesDirect : WeaponProperties
{
    protected override void SetExplosionValues()
    {
        ExplosionDirect explosionDirect = Get<ExplosionDirect>.FromChild(_prefab.gameObject, true);
        explosionDirect.DamageValue = _damageValue;
        explosionDirect.DestructDamageValue = _destructDamage;
    }

    protected override void SetCollisionValues()
    {
        
    }
}
