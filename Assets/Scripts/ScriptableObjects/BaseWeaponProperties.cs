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

    [Header("Weapons type")]
    public int _index;
    public int _value;
    public int _damageValue;
    public int _minutes;
    public int _seconds;
    public string _radius;
    public string _weaponType;

    [Header("Bullet specs")]
    [Range(3, 50)] public float _bulletMaxForce;
    [Range(1, 10)] public float _bulletForceMaxSpeed;

    [Header("Support or props type")]
    public string _supportType;
}
