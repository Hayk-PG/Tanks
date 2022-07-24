using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public IScore OwnerScore { get; set; }
    public float Distance { get; set; }

    [SerializeField] private float _radius;
    [SerializeField] private float _maxDamageValue;
    private int _currentDamageValue;
    private float _percentage;
    private float _distanceFactorPercentage;
    private float _magnitude;
    private Collider[] _colliders;
    private List<IDamage> _iDamages;
    private GameManagerBulletSerializer _gameManagerBulletSerializer;

    
    private void Awake()
    {
        _iDamages = new List<IDamage>();
        _colliders = Physics.OverlapSphere(transform.position, _radius);
        _gameManagerBulletSerializer = FindObjectOfType<GameManagerBulletSerializer>();
    }

    private void Start()
    {
        for (int i = 0; i < _colliders.Length; i++)
        {
            _magnitude = (transform.position - _colliders[i].transform.position).magnitude;
            GetIDamage(Get<IDamage>.From(_colliders[i].gameObject));           
        }
    }  

    private void GetIDamage(IDamage iDamage)
    {
        Conditions<IDamage>.CheckNull(iDamage, null, () => CheckDuplicates(iDamage));
    }

    private void CheckDuplicates(IDamage iDamage)
    {
        Conditions<bool>.Compare(!_iDamages.Contains(iDamage), _iDamages.Contains(iDamage), () => Damage(iDamage), null, null, null);
    }

    private void Damage(IDamage iDamage)
    {
        _iDamages.Add(iDamage);

        _percentage = 100 - Mathf.InverseLerp(0, _radius, _magnitude) * 100;
        _distanceFactorPercentage = 100 - Distance;
        _currentDamageValue = Mathf.RoundToInt((_maxDamageValue / 100 * _percentage) / 100 * _distanceFactorPercentage);
        Conditions<bool>.Compare(_currentDamageValue * 10 > 0, ()=> DamageAndScore(iDamage), null);
    }

    private void DamageAndScore(IDamage iDamage)
    {
        int scoreValue = (_currentDamageValue * 100) + Mathf.FloorToInt(Distance * 10);
        int hitEnemyAndGetScoreValue = _currentDamageValue * 10;
        Conditions<bool>.Compare(MyPhotonNetwork.IsOfflineMode,
        () => DamageAndScoreInOfflineMode(iDamage, _currentDamageValue, scoreValue, hitEnemyAndGetScoreValue),
        () => DamageAndScoreInOlineMode(iDamage, _currentDamageValue, scoreValue, hitEnemyAndGetScoreValue));
    }

    private void DamageAndScoreInOfflineMode(IDamage iDamage, int damageValue, int scoreValue, int hitEnemyAndGetScoreValue)
    {
        iDamage.Damage(damageValue);
        OwnerScore?.GetScore(scoreValue, iDamage);
        OwnerScore?.HitEnemyAndGetScore(hitEnemyAndGetScoreValue, iDamage);
    }

    private void DamageAndScoreInOlineMode(IDamage iDamage, int damageValue, int scoreValue, int hitEnemyAndGetScoreValue)
    {
        _gameManagerBulletSerializer.CallDamageAndScoreRPC(iDamage, OwnerScore, _currentDamageValue, scoreValue, hitEnemyAndGetScoreValue);
    }
}
