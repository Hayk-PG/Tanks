using UnityEngine;

public class BaseWeaponProperties<T> : ScriptableObject
{
    [Header("Prefab")]
    public T _prefab;

    [Header("Index & value")]
    public int _ammoIndex;
    public int _value;
    public string _title;

    [Header("Hud")]
    public Sprite _icon;
    public AmmoTypeStars _ammoTypeStars;
    public int _unlockPoints;
}
