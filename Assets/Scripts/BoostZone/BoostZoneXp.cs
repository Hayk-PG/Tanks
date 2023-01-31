using UnityEngine;

public class BoostZoneXp : BaseBoostZoneFeatures
{
    private ScoreController _scoreController;



    protected override void OnTriggerEnter(GameObject player)
    {
        if (_scoreController == null)
            _scoreController = Get<ScoreController>.From(player);

        _scoreController?.BoostXp(true);
    }

    protected override void OnTriggerExit(GameObject player)
    {
        _scoreController?.BoostXp(false);
        _scoreController = null;
    }
}
