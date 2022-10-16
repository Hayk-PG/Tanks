using System;

public class AddNewWeaponContent : IWoodBoxContent
{
    private WeaponProperties _weaponProperties;
    private NewWeaponFromWoodBox _newWeaponFromWoodBox;

    private bool _isUsed;

    public Action<WeaponProperties> OnNewWeaponTaken { get; set; }


    public AddNewWeaponContent(WeaponProperties weaponProperties, NewWeaponFromWoodBox newWeaponFromWoodBox)
    {
        _weaponProperties = weaponProperties;
        _newWeaponFromWoodBox = newWeaponFromWoodBox;
    }

    public void Use(TankController tankController, Tab_WoodboxContent tab_WoodboxContent)
    {
        if(tankController.BasePlayer != null && !_isUsed)
        {
            _newWeaponFromWoodBox.SubscribeToWoodBoxEvent(this);
            OnNewWeaponTaken?.Invoke(_weaponProperties);
            _isUsed = true;
        }
    }
}
