
public class NewAwailableWeaponSoundsController : UIBaseSoundController
{
    private AmmoTabButtonNotification _ammoTabButtonNotification;


    private void Awake()
    {
        _ammoTabButtonNotification = FindObjectOfType<AmmoTabButtonNotification>();
    }

    private void OnEnable()
    {
        _ammoTabButtonNotification.OnNewAwailableWeaponNotification += OnNewAwailableWeaponNotification;
    }

    private void OnDisable()
    {
        _ammoTabButtonNotification.OnNewAwailableWeaponNotification -= OnNewAwailableWeaponNotification;
    }

    private void OnNewAwailableWeaponNotification()
    {
        PlaySoundFX(0);
    }
}
