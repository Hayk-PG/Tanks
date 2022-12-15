using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable objects/Weapons/New weapon")]
public class WeaponProperties : BaseWeaponProperties<BulletController>
{
    internal struct RandomType
    {
        internal int?[] _damageValues;
        internal int?[] _destructDamageValues;
        internal int?[] _bulletMaxForceVaues;
        internal int?[] _bulletForceMaxSpeedValues;
        internal float?[] _radiusValues;
    }

    private void Randomizer(RandomType randomType)
    {
        if(randomType._damageValues != null) 
            _damageValue = Random.Range(randomType._damageValues[0].Value, randomType._damageValues[1].Value);

        if (randomType._destructDamageValues != null)
            _destructDamage = Random.Range(randomType._destructDamageValues[0].Value, randomType._destructDamageValues[1].Value);

        if (randomType._bulletMaxForceVaues != null)
            _bulletMaxForce = Random.Range(randomType._bulletMaxForceVaues[0].Value, randomType._bulletMaxForceVaues[1].Value);

        if (randomType._bulletForceMaxSpeedValues != null)
            _bulletForceMaxSpeed = Random.Range(randomType._bulletForceMaxSpeedValues[0].Value, randomType._bulletForceMaxSpeedValues[1].Value);

        if(randomType._radiusValues != null)
        {
            float radiusRound = Mathf.Round(Random.Range(randomType._radiusValues[0].Value, randomType._radiusValues[1].Value) * 100) * 0.01f;
            _radius = radiusRound;
        }
    }

    private void SetExplosionValues()
    {
        Explosion[] explosion = _prefab.GetComponentsInChildren<Explosion>(true);
        explosion[0].RadiusValue = _radius;
        explosion[0].DamageValue = _damageValue;
    }

    private void SetCollisionValues()
    {
        BulletCollision bulletCollision = Get<BulletCollision>.From(_prefab.gameObject);

        if (bulletCollision != null)
        {
            bulletCollision.DestructDamage = _destructDamage;
            bulletCollision.TileParticleIndex = _tileParticleIndex;
        }
    }

    public void Randomize()
    {
        switch (_type)
        {
            case Type.Light:

                RandomType randomLight = new RandomType
                {
                    _damageValues = new int?[2] { 10, 30 },
                    _destructDamageValues = new int?[2] { 10, 29 },
                    _radiusValues = new float?[2] { 0.3f, 0.4f }
                };

                Randomizer(randomLight);               
                break;

            case Type.Medium:

                RandomType randomMedium = new RandomType
                {
                    _damageValues = new int?[2] { 30, 63 },
                    _destructDamageValues = new int?[2] { 29, 65 },
                    _radiusValues = new float?[2] { 0.4f, 0.63f }
                };

                Randomizer(randomMedium);
                break;

            case Type.Heavy:

                RandomType randomHeavy = new RandomType
                {
                    _damageValues = new int?[2] { 63, 100 },
                    _destructDamageValues = new int?[2] { 65, 100 },
                    _radiusValues = new float?[2] { 0.63f, 1f }
                };

                Randomizer(randomHeavy);
                break;
        }

        switch (_range)
        {
            case Range.Long:

                RandomType randomLong = new RandomType
                {
                    _bulletMaxForceVaues = new int?[2] { 17, 20 },
                    _bulletForceMaxSpeedValues = new int?[2] { 6, 10 }
                };

                Randomizer(randomLong);
                break;

            case Range.Medium:

                RandomType randomMedium = new RandomType
                {
                    _bulletMaxForceVaues = new int?[2] { 13, 17 },
                    _bulletForceMaxSpeedValues = new int?[2] { 4, 5 }
                };

                Randomizer(randomMedium);
                break;

            case Range.Close:

                RandomType randomClose = new RandomType
                {
                    _bulletMaxForceVaues = new int?[2] { 7, 13 },
                    _bulletForceMaxSpeedValues = new int?[2] { 1, 3 }
                };

                Randomizer(randomClose);
                break;
        }
    }

    private void DefineRangeType()
    {
        string longRange = " LR";
        string mediumRange = " MR";
        string closeRange = " CR";
        string fixedName = "";

        string range = _bulletMaxForce >= 17 ? longRange :
                       _bulletMaxForce >= 13 && _bulletMaxForce < 17 ? mediumRange :
                       _bulletMaxForce >= 7 && _bulletMaxForce < 13 ? closeRange : "fsdfsdfsdf";

        if (_weaponType.Contains(longRange))
        {
            fixedName = _weaponType.Substring(0, _weaponType.Length - mediumRange.Length);
            _weaponType = fixedName;
        }

        if (_weaponType.Contains(mediumRange))
        {
            fixedName = _weaponType.Substring(0, _weaponType.Length - mediumRange.Length);
            _weaponType = fixedName;
        }

        if (_weaponType.Contains(closeRange))
        {
            fixedName = _weaponType.Substring(0, _weaponType.Length - mediumRange.Length);
            _weaponType = fixedName;
        }

        _weaponType += range;
    }

    public void OnClickSetWeaponProperties()
    {
        if (_type == Type.Light)
            _tileParticleIndex = 1000;

        SetExplosionValues();
        SetCollisionValues();
        DefineRangeType();

#if UNITY_EDITOR
        PrefabUtility.SavePrefabAsset(_prefab.gameObject);
#endif
    }
}
