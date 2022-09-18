using UnityEngine;

public class BaseWeaponProperties<T> : ScriptableObject
{
    [Header("Prefab")]
    public T _prefab;

    [Header("Common")]
    public ButtonType _buttonType;
    public Sprite _icon;
    public AmmoTypeStars _ammoTypeStars;
    public int _requiredScoreAmmount;

    public enum Type { Light, Medium, Heavy }
    public enum Range { Close, Medium, Long}
    
    [Header("Weapons type")]
    public int _index;
    public int _value;  
    [Range(0, 10)]
    public int _minutes;
    [Range(0, 60)]
    public int _seconds;
    public Type _type;
    public Range _range;
  
    [Range(0, 70)]
    public int _damageValue;
    [Range(0.3f, 0.4f)]
    public float _radius;
    [Range(0, 20)]
    public int _destructDamage;    
    [Range(3, 50)] public float _bulletMaxForce;
    [Range(1, 10)] public float _bulletForceMaxSpeed;
    public int _tileParticleIndex;   
    public string _weaponType;  

    [Header("Support or props type")]
    public string _supportType;
}
