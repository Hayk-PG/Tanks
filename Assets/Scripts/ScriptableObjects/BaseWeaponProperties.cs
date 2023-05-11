using UnityEngine;

public class BaseWeaponProperties<T> : ScriptableObject
{
    public enum Type { Light, Medium, Heavy }
    public enum Range { Close, Medium, Long}

    [Header("------PROJECTILE------")]
    [Space]

    public T _prefab;
    
    [Space]
    public Type _type;

    public Range _range;
    
    [Space] [Range(10, 100)] 
    public int _damageValue;

    [Range(10, 100)]
    public int _destructDamage; 

    public int _tileParticleIndex;  

    [Space] [Range(0.3f, 1f)] 
    public float _radius;
        
    [Range(1, 20)] 
    public float _bulletMaxForce;

    [Range(1, 10)] 
    public float _bulletForceMaxSpeed;

    [Range(0, 100)] 
    public float _gravityForcePercentage;

    [Range(0, 100)]
    public float _windForcePercentage;
     
    [Space]
    public string _weaponType;  

    [Header("------AMMO BUTTON------")]
    [Space]
    [Space]

    public ButtonType _buttonType;
    
    public AmmoTypeStars _ammoTypeStars;

    public Sprite _icon;

    [Space]
    public int _requiredPointsAmmount;
    public int _requiredPointsIncrementAmount;
    public int _index;
    public int _value;
    public int _refillAmount;
    
    [Range(0, 10)] 
    public int _minutes;

    [Range(0, 60)] 
    public int _seconds;

    [Space]
    public bool _isReusable;

    [Space]
    public string description;
    public string _supportType;
}
