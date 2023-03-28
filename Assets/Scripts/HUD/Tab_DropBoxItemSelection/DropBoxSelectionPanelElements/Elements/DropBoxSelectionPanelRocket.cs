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
        DropBoxSelectionHandler.RaiseEvent(DropBoxItemType.Rocket, new object[] { Weapon, Id, NegativePrice });

        CanUse = false;
    }
}
