using UnityEngine;
using System;

public class DropBoxSelectionPanelRocket : BaseDropBoxSelectionPanelElement
{
    [SerializeField] [Space]
    private WeaponProperties _weapon;

    public WeaponProperties Weapon => _weapon;

    public int Id { get; private set; }

    public event Action<WeaponProperties, int, int> onRocket;


    private void Awake()
    {
        Id = transform.GetSiblingIndex();

        CanUse = Weapon != null;
    }

    protected override void Use()
    {
        onRocket?.Invoke(Weapon, Id, -_price);

        CanUse = false;
    }
}
