using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable objects/Weapons/New weapon")]
public class WeaponProperties : BaseWeaponProperties<BulletController>
{
    internal struct RandomRanges
    {
        internal int[] _rangeDamageValues;
        internal int[] _rangeDestructDamageValues;
        internal int[] _rangeBulletMaxForceVaues;
        internal int[] _rangeBulletForceMaxSpeedValues;
        internal float[] _rangeRadiusValues;

        internal RandomRanges(int[] rangeDamageValues, int[] rangeDestructDamageValues, int[] rangeBulletMaxForceVaues, int[] rangeBulletForceMaxSpeedValues, float[] rangeRadiusValues)
        {
            _rangeDamageValues = rangeDamageValues;
            _rangeDestructDamageValues = rangeDestructDamageValues;
            _rangeBulletMaxForceVaues = rangeBulletMaxForceVaues;
            _rangeBulletForceMaxSpeedValues = rangeBulletForceMaxSpeedValues;
            _rangeRadiusValues = rangeRadiusValues;
        }
    }

    private void Randomizer(RandomRanges randomRanges)
    {
        _damageValue = Random.Range(randomRanges._rangeDamageValues[0], randomRanges._rangeDamageValues[1]);
        _destructDamage = Random.Range(randomRanges._rangeDestructDamageValues[0], randomRanges._rangeDestructDamageValues[1]);
        _bulletMaxForce = Random.Range(randomRanges._rangeBulletMaxForceVaues[0], randomRanges._rangeBulletMaxForceVaues[1]);
        _bulletForceMaxSpeed = Random.Range(randomRanges._rangeBulletForceMaxSpeedValues[0], randomRanges._rangeBulletForceMaxSpeedValues[1]);

        float radiusRound = Mathf.Round(Random.Range(randomRanges._rangeRadiusValues[0], randomRanges._rangeRadiusValues[1]) * 100) * 0.01f;
        _radius = radiusRound;
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
        bulletCollision.DestructDamage = _destructDamage;
        bulletCollision.TileParticleIndex = _tileParticleIndex;        
    }

    public void Randomize()
    {
        switch (_type)
        {
            case Type.Light:
                RandomRanges randomLight = new RandomRanges
                    (
                    new int[2] { 10, 25 },
                    new int[2] { 3, 10 },
                    new int[2] { 16, 20 },
                    new int[2] { 6, 10 },
                    new float[2] { 0.3f, 0.5f }
                    );
                Randomizer(randomLight);
                break;

            case Type.Medium:
                RandomRanges randomMedium = new RandomRanges
                    (
                    new int[2] { 25, 40 },
                    new int[2] { 10, 15 },
                    new int[2] { 13, 16 },
                    new int[2] { 5, 6 },
                    new float[2] { 0.2f, 0.3f }
                    );
                Randomizer(randomMedium);
                break;

            case Type.Heavy:
                RandomRanges randomHeavy = new RandomRanges
                    (
                    new int[2] { 40, 60 },
                    new int[2] { 15, 20 },
                    new int[2] { 9, 13 },
                    new int[2] { 2, 5 },
                    new float[2] { 0.1f, 0.2f }
                    );
                Randomizer(randomHeavy);
                break;
        }
    }

    public void OnClickSetWeaponProperties()
    {
        SetExplosionValues();
        SetCollisionValues();

#if UNITY_EDITOR
        PrefabUtility.SavePrefabAsset(_prefab.gameObject);
#endif
    }
}
