using UnityEngine;

public class BoostZoneSafe : BaseBoostZoneFeatures
{
    private HealthController _healthController;



    protected override void OnTriggerEnter(GameObject player)
    {
        if (_healthController == null)
            _healthController = Get<HealthController>.From(player);

        _healthController?.BoostSafeZone(true);
    }

    protected override void OnTriggerExit(GameObject player)
    {
        _healthController?.BoostSafeZone(false);
        _healthController = null;
    }
}
