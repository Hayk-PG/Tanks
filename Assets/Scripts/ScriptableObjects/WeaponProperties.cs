using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable objects/Weapons/New weapon")]
public class WeaponProperties : ScriptableObject
{
    [Header("Shoot parameters")]
    public BulletController _bulletPrefab;

    public int _ammoIndex;
    public int _value;

    [Header("HUD")]
    public Sprite _icon;
    public AmmoTypeStars _ammoTypeStars;
    public int _unlockPoints;
}
