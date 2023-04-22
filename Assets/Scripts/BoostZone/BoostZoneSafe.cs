public class BoostZoneSafe : BaseBoostZoneFeatures
{
    private HealthController _healthController;


    protected override void OnTriggerEnter(TankController tankController)
    {
        _healthController = Get<HealthController>.From(tankController.gameObject);
        _healthController?.BoostSafeZone(true);
    }

    protected override void OnTriggerExit(TankController tankController)
    {
        _healthController?.BoostSafeZone(false);
        _healthController = null;
    }
}
