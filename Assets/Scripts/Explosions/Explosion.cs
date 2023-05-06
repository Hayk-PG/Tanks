using System.Collections.Generic;
using UnityEngine;

public class Explosion : BaseExplosion
{   
    [SerializeField] 
    protected float _radius, _maxDamageValue;
    protected float _percentage;
    protected float _distanceFactorPercentage;
    protected float _magnitude;

    protected int _currentDamageValue;
     
    protected Collider[] _colliders;
    protected List<IDamage> _iDamages;

    public override float RadiusValue
    {
        get => _radius;
        set => _radius = value;
    }
    public override float DamageValue
    {
        get => _maxDamageValue;
        set => _maxDamageValue = value;
    }





    protected virtual void Awake()
    {
        _iDamages = new List<IDamage>();

        _colliders = Physics.OverlapSphere(transform.position, _radius);
    }

    protected override void Start()
    {
        base.Start();

        ApplyDamageToColliders();
    }

    protected virtual void ApplyDamageToColliders()
    {
        for (int i = 0; i < _colliders.Length; i++)
        {
            _magnitude = (transform.position - _colliders[i].transform.position).magnitude;

            TryGetDamagable(Get<IDamage>.From(_colliders[i].gameObject));
        }
    }

    protected virtual void TryGetDamagable(IDamage iDamage)
    {
        Conditions<IDamage>.CheckNull(iDamage, null, () => CheckDamagablesDuplicates(iDamage));
    }

    protected virtual void CheckDamagablesDuplicates(IDamage iDamage)
    {
        Conditions<bool>.Compare(!_iDamages.Contains(iDamage), _iDamages.Contains(iDamage), () => CalculateDamage(iDamage), null, null, null);
    }

    protected virtual void CalculateDamage(IDamage iDamage)
    {
        _iDamages.Add(iDamage);

        _percentage = 100 - Mathf.InverseLerp(0, _radius, _magnitude) * 100;

        _distanceFactorPercentage = 100 - Distance;

        _currentDamageValue = Mathf.RoundToInt((_maxDamageValue / 100 * _percentage) / 100 * _distanceFactorPercentage);

        Conditions<bool>.Compare(_currentDamageValue * 10 > 0, ()=> CalculateScoreAndApplyDamageAndGetScored(iDamage), null);
    }

    protected virtual void CalculateScoreAndApplyDamageAndGetScored(IDamage iDamage)
    {
        int hitScore = (_currentDamageValue * 100);
        int distanceBonus = Mathf.FloorToInt(Distance * 10);

        Conditions<bool>.Compare(MyPhotonNetwork.IsOfflineMode, () => { DamageAndScoreInOfflineMode(iDamage, _currentDamageValue, new int[2] { hitScore, distanceBonus }); },
                                                                () => { DamageAndScoreInOlineMode(iDamage, _currentDamageValue, new int[2] { hitScore, distanceBonus }); });
    }

    protected virtual void DamageAndScoreInOfflineMode(IDamage iDamage, int damageValue, int[] scores)
    {
        //Preventing self-damage for Ai

        if (IDamageAi == iDamage)
            return;

        ApplyDamage(iDamage, damageValue);

        GetScored(iDamage, scores);
    }

    protected virtual void ApplyDamage(IDamage iDamage, int damageValue, bool ignoreArmor = false) => iDamage.Damage(damageValue, ignoreArmor);

    protected virtual void GetScored(IDamage iDamage, int[] scores) => Score(iDamage, scores);

    protected virtual void DamageAndScoreInOlineMode(IDamage iDamage, int damageValue, int[] scores, bool ignoreArmor = false)
    {
        GameSceneObjectsReferences.GameManagerBulletSerializer.CallDamageAndScoreRPC(iDamage, OwnerScore, _currentDamageValue, scores, 0, ignoreArmor);
    }
}
