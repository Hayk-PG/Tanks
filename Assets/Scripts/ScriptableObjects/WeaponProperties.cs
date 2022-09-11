using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable objects/Weapons/New weapon")]
public class WeaponProperties : BaseWeaponProperties<BulletController>
{
    private void SetExplosionValues()
    {
        Explosion[] explosion = _prefab.GetComponentsInChildren<Explosion>(true);
        explosion[0].RadiusValue = _radius;
        explosion[0].DamageValue = _damageValue;
    }

    private void SetCollisionValues()
    {
        BulletCollision bulletCollision = Get<BulletCollision>.From(_prefab.gameObject);
        bulletCollision.DestructDamage = _destructDamage;
        bulletCollision.TileParticleIndex = _tileParticleIndex;
        Debug.Log(bulletCollision.DestructDamage);
    }

    public void OnClickSetWeaponProperties()
    {
        SetExplosionValues();
        SetCollisionValues();
    }
}
