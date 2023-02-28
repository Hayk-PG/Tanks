using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable objects/Weapons/New weapon")]
public class WeaponProperties : BaseWeaponProperties<BaseBulletController>
{
    protected struct RandomType
    {
        internal int?[] _damageValues;
        internal int?[] _destructDamageValues;
        internal int?[] _bulletMaxForceVaues;
        internal int?[] _bulletForceMaxSpeedValues;
        internal float?[] _radiusValues;
        internal float?[] _gravityForcePercentage;
        internal float?[] _windForcePercentage;
    }

    protected void Randomizer(RandomType randomType)
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

        if (randomType._gravityForcePercentage != null)
            _gravityForcePercentage = Mathf.Round(Random.Range(randomType._gravityForcePercentage[0].Value, randomType._gravityForcePercentage[1].Value));

        if (randomType._windForcePercentage != null)
            _windForcePercentage = Mathf.Round(Random.Range(randomType._windForcePercentage[0].Value, randomType._windForcePercentage[1].Value));
    }

    protected virtual void SetExplosionValues()
    {
        Explosion[] explosion = _prefab.GetComponentsInChildren<Explosion>(true);
        explosion[0].RadiusValue = _radius;
        explosion[0].DamageValue = _damageValue;
    }

    protected virtual void SetCollisionValues()
    {
        BaseBulletCollision baseBulletCollision = Get<BaseBulletCollision>.From(_prefab.gameObject);

        if (baseBulletCollision != null)
        {
            baseBulletCollision.DestructDamage = _destructDamage;
            baseBulletCollision.TileParticleIndex = _tileParticleIndex;
        }
    }

    protected virtual void SetForcePercentages()
    {
        BaseBulletVelocity baseBulletVelocity = Get<BaseBulletVelocity>.From(_prefab.gameObject);

        if(baseBulletVelocity != null)
        {
            baseBulletVelocity.GravityForcePercentage = _gravityForcePercentage;
            baseBulletVelocity.WindForcePercentage = _windForcePercentage;
        }
    }

    public void Randomize()
    {
        switch (_type)
        {
            case Type.Light:

                RandomType randomLight = new RandomType
                {
                    _damageValues = new int?[] { 10, 30 },
                    _destructDamageValues = new int?[] { 10, 29 },
                    _radiusValues = new float?[] { 0.3f, 0.4f },
                    _gravityForcePercentage = new float?[] { 0, 35 },
                    _windForcePercentage = new float?[] { 75, 100 }
                };

                Randomizer(randomLight);               
                break;

            case Type.Medium:

                RandomType randomMedium = new RandomType
                {
                    _damageValues = new int?[] { 30, 63 },
                    _destructDamageValues = new int?[] { 29, 65 },
                    _radiusValues = new float?[] { 0.4f, 0.63f },
                    _gravityForcePercentage = new float?[] { 40, 70 },
                    _windForcePercentage = new float?[] { 40, 70 }
                };

                Randomizer(randomMedium);
                break;

            case Type.Heavy:

                RandomType randomHeavy = new RandomType
                {
                    _damageValues = new int?[] { 63, 100 },
                    _destructDamageValues = new int?[] { 65, 100 },
                    _radiusValues = new float?[] { 0.63f, 1f },
                    _gravityForcePercentage = new float?[] { 75, 100 },
                    _windForcePercentage = new float?[] { 0, 35 }
                };

                Randomizer(randomHeavy);
                break;
        }

        switch (_range)
        {
            case Range.Long:

                RandomType randomLong = new RandomType
                {
                    _bulletMaxForceVaues = new int?[] { 17, 20 },
                    _bulletForceMaxSpeedValues = new int?[] { 6, 10 }
                };

                Randomizer(randomLong);
                break;

            case Range.Medium:

                RandomType randomMedium = new RandomType
                {
                    _bulletMaxForceVaues = new int?[] { 13, 17 },
                    _bulletForceMaxSpeedValues = new int?[] { 4, 5 }
                };

                Randomizer(randomMedium);
                break;

            case Range.Close:

                RandomType randomClose = new RandomType
                {
                    _bulletMaxForceVaues = new int?[] { 7, 13 },
                    _bulletForceMaxSpeedValues = new int?[] { 1, 3 }
                };

                Randomizer(randomClose);
                break;
        }

        SetRandomUnlockTime();
    }

    protected void DefineRangeType()
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

        //_weaponType += range;
    }

    protected void SetRandomUnlockTime()
    {
        if (_index == 0)
            return;

        _seconds = Random.Range(0, 61);
        _minutes = Random.Range(0, 11);
    }

    public void OnClickSetWeaponProperties()
    {
        if (_type == Type.Light)
            _tileParticleIndex = 1000;

        SetExplosionValues();
        SetCollisionValues();
        SetForcePercentages();
        DefineRangeType();

#if UNITY_EDITOR
        PrefabUtility.SavePrefabAsset(_prefab.gameObject);
#endif
    }
}
