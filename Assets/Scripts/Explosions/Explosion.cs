using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField]
    private float _radius;
    private Collider[] _colliders;
  
    private int _currentDamageValue;
    [SerializeField]
    private float _maxDamageValue;
    private float _percentage;
    private float _magnitude;

    private List<IDamage> _iDamages;

    
    private void Awake()
    {
        _iDamages = new List<IDamage>();
        _colliders = Physics.OverlapSphere(transform.position, _radius);
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
        _percentage = 100 - Mathf.InverseLerp(0, _radius, _magnitude) * 100;
        _currentDamageValue = Mathf.RoundToInt(_maxDamageValue / 100 * _percentage);
        print(_percentage + "/" + _magnitude + "/" + _currentDamageValue);
        iDamage.Damage(_currentDamageValue);
        _iDamages.Add(iDamage);

    }
}
