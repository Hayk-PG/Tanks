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
    public string _weaponMass;
    public string _weaponType;

    [Header("Support or props type")]
    public string _supportType;
}
