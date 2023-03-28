using UnityEngine;

public class DropBoxSelectionPanelRocket : BaseDropBoxSelectionPanelElement
{
    [SerializeField] [Space]
    private WeaponProperties _weapon;

    public WeaponProperties Weapon => _weapon;

    public int Id { get; private set; }



    private void Awake()
    {
        Id = transform.GetSiblingIndex();

        CanUse = Weapon != null;
    }

    protected override void Use()
    {
        _data[0] = Weapon;
        _data[1] = Id;
        _data[2] = NegativePrice;

        DropBoxSelectionHandler.RaiseEvent(DropBoxItemType.Rocket, _data);

        CanUse = false;
    }
}
